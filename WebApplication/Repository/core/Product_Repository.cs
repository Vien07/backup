using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using X.PagedList;
using System.Globalization;
using CMS.Areas.Product.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Product;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static CMS.Services.ExtensionServices;
using CMS.Areas.Promotion.Models;
using Microsoft.Extensions.Caching.Memory;
using Coravel.Cache;

namespace CMS.Repository
{
    public class Product_Repository : IProduct_Repository
    {
        private string DateFormat = "";
        private string PageLimit = "";

        private readonly ICommonServices _common;
        private readonly IMemoryCache _memory;

        private readonly DBContext _dbContext;

        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string KeyDateFormat = ConstantStrings.KeyDateFormat;
        private string UrlProductImages = ConstantStrings.UrlProductImages;
        private string UrlCustomerImages = ConstantStrings.UrlCustomerImages;
        private string Thumb = ConstantStrings.Thumb;
        private string Fullmages = ConstantStrings.Fullmages;
        private string formatBase64 = ConstantStrings.formatBase64;
        private int ProductId = ConstantStrings.ProductId;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;
        private string UrlProductTypeImages = ConstantStrings.UrlProductTypeImages;
        private string UrlProductColorImages = ConstantStrings.UrlProductColorImages;

        public Product_Repository(DBContext dbContext, ICommonServices common, IMemoryCache memory)
        {
            _memory = memory;
            _dbContext = dbContext;
            _common = common;
            DateFormat = _common.GetConfigValue(KeyDateFormat);
            PageLimit = _common.GetConfigValue(KeyPageLimit);
        }
        public async Task<List<ProductDto>> GetList(string lang, int page, string sortby)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                var model = await (from a in _dbContext.ProductDetails
                                   join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   orderby a.Level
                                   select new ProductDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Code = a.Code,
                                       Slug = b.Slug,
                                       IsHot = a.IsHot,
                                       Level = a.Level,
                                       Price = 0,
                                       PriceString = "",
                                   }).ToListAsync();
                foreach (var item in model)
                {
                    decimal price = 0;
                    var temp = _dbContext.ProductCate_ProductDetails.Where(x => x.ProductDetailPid == item.Pid).FirstOrDefault();
                    if (temp != null)
                    {
                        price = temp.Price;
                    }
                    item.Price = price;
                    item.PriceString = _common.ConvertFormatMoney(price);
                }
                return model;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return new List<ProductDto>();
            }
        }
        public async Task<List<ProductDto>> GetListBySlug(string lang, int page, string cate, string sortby)
        {
            try
            {
                //var promotions = _common.getAllPromotions();
                //var options = _common.getAllProductOptions();

                CultureInfo vi = new CultureInfo(lang);
                var productCate = await GetCate(cate, lang);

                var model = await (from a in _dbContext.ProductDetails
                                   join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                   //join c in _dbContext.ProductOption_ProductDetails on a.Pid equals c.ProductDetailPid
                                   join d in _dbContext.ProductCate_ProductDetails on a.Pid equals d.ProductDetailPid
                                   where (!a.Deleted && a.Enabled && b.LangKey == lang) && d.ProductCatePid == productCate.Pid
                                   orderby a.Order descending
                                   select new ProductDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       //Content = b.Content,
                                       Description = b.Description,
                                       Code = a.Code,
                                       Slug = b.Slug,
                                       //CreateDate = a.CreateDate.Value,
                                       PicThumb = UrlProductImages + Thumb + a.PicThumb,
                                       //IsHot = a.IsHot,
                                       //IsNew = a.IsNew,
                                       //Price = c.Price,
                                       //PriceDiscount = c.PriceDiscount,
                                       //PriceString = _common.ConvertFormatMoney(c.Price),
                                       //PriceDiscountString = _common.ConvertFormatMoney(c.PriceDiscount)
                                   }).ToListAsync();

                model = model.DistinctBy(x => x.Pid).ToList();


                //foreach (var item in model)
                //{
                //    var price = options.Where(x => x.ProductDetailPid == item.Pid).FirstOrDefault();
                //    item.Price = price.Price;
                //    item.PriceDiscount = price.PriceDiscount;
                //    item.PriceString = _common.ConvertFormatMoney(price.Price);
                //    item.PriceDiscountString = _common.ConvertFormatMoney(price.PriceDiscount);

                //    //check promotion
                //    if (promotions.Any())
                //    {
                //        var pricePromotion = promotions.Where(x => x.ProductPid == item.Pid && x.OptionPid == price.ProductOptionPid).FirstOrDefault();
                //        if (pricePromotion != null)
                //        {
                //            item.PriceDiscount = pricePromotion.Price;
                //            item.PriceDiscountString = _common.ConvertFormatMoney(pricePromotion.Price);
                //        }
                //    }
                //}

                //if (!string.IsNullOrEmpty(sortby))
                //{
                //    switch (sortby)
                //    {
                //        case "lowest":
                //            model = model.OrderBy(x => x.PriceDiscount).OrderBy(x => x.Price).ToList();
                //            break;
                //        case "hightest":
                //            model = model.OrderByDescending(x => x.PriceDiscount).OrderByDescending(x => x.Price).ToList();
                //            break;
                //        case "newest":
                //            model = model.OrderBy(x => x.CreateDate).ToList();
                //            break;
                //        case "oldest":
                //            model = model.OrderByDescending(x => x.CreateDate).ToList();
                //            break;
                //        case "az":
                //            model = model.OrderBy(x => x.Title).ToList();
                //            break;
                //        case "za":
                //            model = model.OrderByDescending(x => x.Title).ToList();
                //            break;
                //    }
                //}
                return model;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return new List<ProductDto>();
            }
        }
        public async Task<ProductDto> GetProduct(string slug, string lang)
        {
            try
            {
                //var promotions = _common.getAllPromotions();

                CultureInfo vi = new CultureInfo(lang);
                var model = await (from a in _dbContext.ProductDetails
                                   join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                   join c in _dbContext.ProductCate_ProductDetails on a.Pid equals c.ProductDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.Slug == slug && b.LangKey == lang)
                                   select new ProductDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       //Content2 = b.Content2,
                                       //Material = b.Material,
                                       //Size = a.Size,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       //TagKey = a.TagKey,
                                       //Tiki = a.Tiki,
                                       //IsHot = a.IsHot,
                                       //IsNew = a.IsNew,
                                       //Lazada = a.Lazada,
                                       //Shopee = a.Shopee,
                                       Enabled = a.Enabled,
                                       Code = a.Code,
                                       SlugTagKey = a.SlugTagKey != null ? a.SlugTagKey : "",
                                       PicThumb = UrlProductImages + Thumb + a.PicThumb,
                                       PicFull = UrlProductImages + Fullmages + a.PicThumb,
                                       CateName = _dbContext.MultiLang_ProductCates
                                                   .Where(x => x.ProductCatePid == c.ProductCatePid && x.LangKey == lang)
                                                   .FirstOrDefault().Name,
                                       CateSlug = _dbContext.MultiLang_ProductCates
                                                   .Where(x => x.ProductCatePid == c.ProductCatePid && x.LangKey == lang)
                                                   .FirstOrDefault().Slug,
                                   }).FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(model.TitleSEO))
                {
                    model.TitleSEO = model.Title;
                }
                if (string.IsNullOrEmpty(model.DescriptionSEO))
                {
                    model.DescriptionSEO = model.Description;
                }

                #region add view
                var common = await _dbContext.ProductDetails.Where(p => p.Pid == model.Pid && p.Enabled == true && p.Deleted == false).FirstOrDefaultAsync();
                common.CounterView = common.CounterView + 1;
                await _dbContext.SaveChangesAsync();
                #endregion

                //#region color
                //var colors = await (from a in _dbContext.ProductColors
                //                    join b in _dbContext.MultiLang_ProductColors on a.Pid equals b.ProductColorPid
                //                    where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang)
                //                    orderby a.Order descending
                //                    select new ProductColorDto
                //                    {
                //                        Title = b.Name,
                //                        Code = a.Code,
                //                        Pid = a.Pid,
                //                        PicThumb = UrlProductColorImages + a.PicThumb
                //                    }).ToListAsync();

                //var colorPids = await _dbContext.ProductColor_ProductDetails.Where(x => x.ProductDetailPid == model.Pid).Select(x => x.ProductColorPid).ToListAsync();
                //model.Colors = new List<ProductColorDto>();
                //model.Colors = colors.Where(x => colorPids.Contains(x.Pid)).Distinct().ToList();
                //#endregion

                //#region option
                //var options = await (from a in _dbContext.ProductOptions
                //                     join b in _dbContext.ProductOption_ProductDetails on a.Pid equals b.ProductOptionPid
                //                     join c in _dbContext.MultiLang_ProductOptions on a.Pid equals c.ProductOptionPid
                //                     where !a.Deleted && a.Enabled && b.Status && b.ProductDetailPid == model.Pid && c.LangKey == lang
                //                     orderby a.Order descending
                //                     select new ProductOptionDto
                //                     {
                //                         OptionId = Convert.ToInt32(a.Pid),
                //                         ProductId = Convert.ToInt32(b.Pid),
                //                         Title = c.Name,
                //                         Code = a.Code,

                //                         Price = b.Price,
                //                         PriceDiscount = b.PriceDiscount,
                //                         PriceString = _common.ConvertFormatMoney(b.Price),
                //                         PriceDiscountString = _common.ConvertFormatMoney(b.PriceDiscount),
                //                     }).ToListAsync();
                //model.Options = new List<ProductOptionDto>();
                //foreach (var el in options)
                //{
                //    if (promotions.Any())
                //    {
                //        var pricePromotion = promotions.Where(x => x.ProductPid == model.Pid && x.OptionPid == el.OptionId).FirstOrDefault();
                //        if (pricePromotion != null)
                //        {
                //            el.PriceDiscount = pricePromotion.Price;
                //            el.PriceDiscountString = _common.ConvertFormatMoney(pricePromotion.Price);
                //        }
                //    }
                //}
                //model.Options = options;
                //#endregion

                #region image
                model.ImageMeta = model.PicFull;
                model.ImageList = new List<string>();
                var imgs = await _dbContext.Images_Products.Where(x => x.ProductDetailPid == model.Pid).ToListAsync();

                model.ImageList.Add(model.PicFull);
                foreach (var item in imgs)
                {
                    model.ImageList.Add(UrlProductImages + Fullmages + item.Images);
                }
                #endregion

                #region comment
                model.Comments = new List<ProductCommentDto>();
                model.Comments = LoadComment(model.Pid, 1, "", "");
                #endregion

                return model;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return null;
            }
        }
        public async Task<List<ProductDto>> GetRelateList(string slug, string lang, int limit)
        {
            try
            {
                //var promotions = _common.getAllPromotions();
                //var options = _common.getAllProductOptions();

                var categoryPids = await (from a in _dbContext.ProductDetails
                                          join b in _dbContext.ProductCate_ProductDetails on a.Pid equals b.ProductDetailPid
                                          join c in _dbContext.MultiLang_ProductDetails on a.Pid equals c.ProductDetailPid
                                          where (!a.Deleted && a.Enabled) && (c.Slug == slug) && (c.LangKey == lang)
                                          select b.ProductCatePid).ToListAsync();

                var model = await (from a in _dbContext.ProductDetails
                                   join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                   join c in _dbContext.ProductCate_ProductDetails on a.Pid equals c.ProductDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang && b.Slug != slug) && (categoryPids.Contains(c.ProductCatePid))
                                   orderby a.Order descending
                                   select new ProductDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       //Content = b.Content,
                                       Description = b.Description,
                                       Code = a.Code,
                                       Slug = b.Slug,
                                       //CreateDate = a.CreateDate.Value,
                                       PicThumb = UrlProductImages + Thumb + a.PicThumb,
                                       //IsHot = a.IsHot,
                                       //IsNew = a.IsNew,
                                   }).ToListAsync();
                model = model.DistinctBy(x => x.Pid).Take(limit).ToList();
                //foreach (var item in model)
                //{
                //    var price = options.Where(x => x.ProductDetailPid == item.Pid).FirstOrDefault();
                //    item.Price = price.Price;
                //    item.PriceDiscount = price.PriceDiscount;
                //    item.PriceString = _common.ConvertFormatMoney(price.Price);
                //    item.PriceDiscountString = _common.ConvertFormatMoney(price.PriceDiscount);

                //    //check promotion
                //    if (promotions.Any())
                //    {
                //        var pricePromotion = promotions.Where(x => x.ProductPid == item.Pid && x.OptionPid == price.ProductOptionPid).FirstOrDefault();
                //        if (pricePromotion != null)
                //        {
                //            item.PriceDiscount = pricePromotion.Price;
                //            item.PriceDiscountString = _common.ConvertFormatMoney(pricePromotion.Price);
                //        }
                //    }
                //}

                return model;
            }
            catch (Exception ex)
            {
                return new List<ProductDto>();
            }
        }
        public async Task<List<ProductDto>> GetHotList(string lang, int limit)
        {
            try
            {
                if (limit == 0)
                {
                    limit = 100;
                }

                var promotions = _common.getAllPromotions();
                var options = _common.getAllProductOptions();

                var model = await (from a in _dbContext.ProductDetails
                                   join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   orderby a.IsHot descending, a.Order descending
                                   select new ProductDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PicThumb = UrlProductImages + Thumb + a.PicThumb,
                                       PicFull = UrlProductImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                foreach (var item in model)
                {
                    var price = options.Where(x => x.ProductDetailPid == item.Pid).FirstOrDefault();
                    item.Price = price.Price;
                    item.PriceDiscount = price.PriceDiscount;
                    item.PriceString = _common.ConvertFormatMoney(price.Price);
                    item.PriceDiscountString = _common.ConvertFormatMoney(price.PriceDiscount);

                    //check promotion
                    if (promotions.Any())
                    {
                        var pricePromotion = promotions.Where(x => x.ProductPid == item.Pid && x.OptionPid == price.ProductOptionPid).FirstOrDefault();
                        if (pricePromotion != null)
                        {
                            item.PriceDiscount = pricePromotion.Price;
                            item.PriceDiscountString = _common.ConvertFormatMoney(pricePromotion.Price);
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                return new List<ProductDto>();
            }
        }
        public async Task<ProductCateDto> GetCate(string slug, string lang)
        {
            try
            {
                var model = await (from a in _dbContext.ProductCates
                                   join b in _dbContext.MultiLang_ProductCates on a.Pid equals b.ProductCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang && b.Slug == slug)
                                   orderby a.Order descending
                                   select new ProductCateDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Name,
                                       TitleAlt = b.Name.Replace("\"", "").Replace("'", ""),
                                       Slug = b.Slug,
                                       Description = b.Description,
                                   }).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                return new ProductCateDto();
            }
        }
        public async Task<List<ProductCateDto>> GetCateList(string lang)
        {
            try
            {
                var model = await (from a in _dbContext.ProductCates
                                   join b in _dbContext.MultiLang_ProductCates on a.Pid equals b.ProductCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked && a.ParentId == 0) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new ProductCateDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Name,
                                       TitleAlt = b.Name.Replace("\"", "").Replace("'", ""),
                                       Slug = b.Slug,
                                       Description = b.Description,
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                return new List<ProductCateDto>();
            }
        }
        public async Task<List<ProductCateDto>> GetCateParentList(string lang)
        {
            try
            {
                var model = await (from a in _dbContext.ProductCates
                                   join b in _dbContext.MultiLang_ProductCates on a.Pid equals b.ProductCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked && a.ParentId == 0) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new ProductCateDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Name,
                                       TitleAlt = b.Name.Replace("\"", "").Replace("'", ""),
                                       Slug = b.Slug,
                                       Description = b.Description,
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                return new List<ProductCateDto>();
            }
        }
        public ProductDto GetProductPreview()
        {
            try
            {
                var news = new ProductDto();
                string lang = DefaultLang;
                CultureInfo vi = new CultureInfo(lang);
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == ProductId.ToString()).FirstOrDefault();
                var common = JsonConvert.DeserializeObject<ProductDetail>(model.Obj);
                var detail = JsonConvert.DeserializeObject<List<MultiLang_ProductDetail>>(model.ObjDetail).Where(x => x.LangKey == lang).FirstOrDefault();


                news.Title = detail.Title;
                news.TitleAlt = detail.Title.Replace("\"", "").Replace("'", "");
                news.Enabled = common.Enabled;
                news.Content = detail.Content;
                news.Description = detail.Description;
                news.Pid = common.Pid;

                if (model.IsEditPicThumb)
                {
                    news.PicThumb = UrlPreviewImages + Thumb + model.PicThumb;
                    news.PicFull = UrlPreviewImages + Fullmages + model.PicThumb;
                    news.ImageMeta = news.PicFull;
                }
                else
                {
                    var currentProduct = _dbContext.ProductDetails.Where(x => x.Pid == common.Pid).FirstOrDefault();
                    if (currentProduct != null)
                    {
                        news.PicThumb = UrlProductImages + Thumb + currentProduct.PicThumb;
                        news.PicFull = UrlProductImages + Fullmages + currentProduct.PicThumb;
                        news.ImageMeta = news.PicFull;
                    }
                }

                news.Slug = detail.Slug;
                news.TagKey = common.TagKey;
                return news;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> ChangePrice(int optionId, long productId)
        {
            try
            {
                var promotions = _common.getAllPromotions();

                var option = await (from a in _dbContext.ProductOptions
                                    join b in _dbContext.ProductOption_ProductDetails on a.Pid equals b.ProductOptionPid
                                    where !a.Deleted && a.Enabled && b.Status && a.Pid == optionId && b.ProductDetailPid == productId
                                    orderby a.Order descending
                                    select new ProductOptionDto
                                    {
                                        Price = b.Price,
                                        PriceDiscount = b.PriceDiscount,
                                        PriceString = _common.ConvertFormatMoney(b.Price),
                                        PriceDiscountString = _common.ConvertFormatMoney(b.PriceDiscount),
                                    }).FirstOrDefaultAsync();

                if (promotions.Any())
                {
                    var pricePromotion = promotions.Where(x => x.ProductPid == productId && x.OptionPid == optionId).FirstOrDefault();
                    if (pricePromotion != null)
                    {
                        option.PriceDiscount = pricePromotion.Price;
                        option.PriceDiscountString = _common.ConvertFormatMoney(pricePromotion.Price);
                    }
                }

                return JsonConvert.SerializeObject(option);
            }
            catch
            {
                return "";
            }
        }
        public async Task<bool> InsertComment(int productId, int customerId, string message, int? parentId)
        {
            try
            {
                ProductComment productComment = new ProductComment();
                productComment.Comment = _common.RemoveScripsTag(message);
                productComment.CustomerPid = customerId;
                productComment.ProductDetailPid = productId;
                productComment.ProductDetailPid = productId;
                productComment.ParentId = parentId;
                await _dbContext.ProductComments.AddAsync(productComment);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return false;
            }
        }
        public List<ProductCommentDto> LoadComment(long productId, int page, string filter, string sort)
        {
            try
            {
                if (page < 1)
                {
                    page = 1;
                }

                var models = (from a in _dbContext.ProductComments
                              join c in _dbContext.Customers on a.CustomerPid equals c.Pid
                              where (!c.Deleted && c.Enabled) && (a.ProductDetailPid == productId && a.ParentId == null) && (!a.Deleted && a.Enabled)
                              orderby a.CreateDate descending
                              select new ProductCommentDto
                              {
                                  ProductCommentPid = a.Pid,
                                  CustomerPid = c.Pid,
                                  Comment = a.Comment,
                                  Like = a.Like,
                                  Star = a.Star,
                                  Children = (from child_a in _dbContext.ProductComments
                                              join child_b in _dbContext.Customers on child_a.CustomerPid equals child_b.Pid
                                              where (!child_a.Deleted && child_a.Enabled) && (!child_b.Deleted && child_b.Enabled) && (child_a.ParentId == a.Pid)
                                              orderby child_a.CreateDate ascending
                                              select new ProductCommentDto
                                              {
                                                  ProductCommentPid = child_a.Pid,
                                                  CustomerPid = child_b.Pid,
                                                  Comment = child_a.Comment,
                                                  Like = child_a.Like,
                                                  ReplyName = _dbContext.Customers.Where(x => x.Pid == child_a.ReplyId).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                  FullName = child_b.LastName + " " + child_b.FirstName,
                                                  PicThumb = UrlCustomerImages + child_b.PicThumb,
                                                  CreateDate = child_a.CreateDate.Value.ToString("dd/MM/yyyy HH:mm")
                                              }).ToList(),
                                  FullName = c.LastName + " " + c.FirstName,
                                  PicThumb = UrlCustomerImages + c.PicThumb,
                                  CreateDate = a.CreateDate.Value.ToString("dd/MM/yyyy HH:mm")
                              }).ToList();

                if (!string.IsNullOrEmpty(filter))
                {
                    var star = Convert.ToInt32(filter);
                    models = models.Where(x => x.Star == star).ToList();
                }
                if (!string.IsNullOrEmpty(sort))
                {
                    switch (sort)
                    {
                        case "date_desc":
                            models = models.OrderByDescending(x => x.CreateDate).ToList();
                            break;
                        case "date_asc":
                            models = models.OrderBy(x => x.CreateDate).ToList();
                            break;
                        case "like":
                            models = models.OrderByDescending(x => x.Like).ToList();
                            break;
                    }
                }

                return models;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return null;
            }
        }
    }
}
