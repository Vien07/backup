using CMS.Areas.Product.Models;
using CMS.Areas.Shared.Models;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Common;
using DTO.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using X.PagedList;
using static CMS.Services.ExtensionServices;

namespace CMS.Areas.Product
{
    public class ProductRepository : IProductRepository
    {

        private readonly DBContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly IMemoryCache _memory;
        private readonly ICommonServices _common;
        private string WatermarkActive = "";
        private string WatermarkPicThumbActive = "";

        private string UrlProductImages = ConstantStrings.UrlProductImages;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string Fullmages = ConstantStrings.Fullmages;
        private int ProductId = ConstantStrings.ProductId;
        private int RootProductCatePid = ConstantStrings.RootProductCatePid;
        private string _productCode = "";
        public ProductRepository(DBContext dbContext,
                             IFileServices fileHelper, ICommonServices common, IMemoryCache memory)
        {
            _dbContext = dbContext;
            _fileServices = fileHelper;
            _common = common;
            _memory = memory;
            _productCode = _common.GetConfigValue(ConstantStrings.KeyProductCode);
            WatermarkActive = _common.GetConfigValue(ConstantStrings.KeyWatermarkActive);
            WatermarkPicThumbActive = _common.GetConfigValue(ConstantStrings.KeyWatermarkPicThumbActive);
        }
        public dynamic LoadData(SearchDto search)
        {
            try
            {
                int cate = search.Cate != null ? Convert.ToInt32(search.Cate) : 0;
                List<dynamic> listData = new List<dynamic>();
                var data = (from a in _dbContext.ProductDetails
                            join b in _dbContext.ProductCate_ProductDetails on a.Pid equals b.ProductDetailPid into ab
                            from x in ab.DefaultIfEmpty()

                            join c in _dbContext.MultiLang_ProductDetails on a.Pid equals c.ProductDetailPid into cab
                            from y in cab.DefaultIfEmpty()
                            where (x.ProductCatePid == cate || cate == 0) && (a.Enabled == search.Enable || search.Enable == null)
                                                    && a.Deleted == false && y.LangKey == DefaultLang
                            select new
                            {
                                Pid = a.Pid,
                                CounterView = a.CounterView,
                                Order = a.Order,
                                Enabled = a.Enabled,
                                IsHot = a.IsHot,
                                IsNew = a.IsNew,
                                PicThumb = a.PicThumb,
                                Title = y.Title,
                                Slug = y.Slug,
                                Description = y.Description,
                                Level = a.Level,
                                Cycle = a.Cycle,
                            }).Distinct().ToList().FilterSearch(new string[] { "Title", "Description" }, search.Key);

                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Title = item.Title;
                    child.Slug = item.Slug;
                    child.CounterView = item.CounterView;
                    child.PicThumb = UrlProductImages + item.PicThumb;
                    child.Pid = item.Pid;
                    child.Order = item.Order;
                    child.Enabled = item.Enabled;
                    child.IsHot = item.IsHot;
                    child.IsNew = item.IsNew;
                    child.Level = item.Level;
                    child.Cycle = item.Cycle;
                    listData.Add(child);
                }
                PagedList<dynamic> dataPaging = new PagedList<dynamic>(listData.OrderByDescending(p => p.Order).ToList(), search.Page, search.PageNumber);
                var rs = Newtonsoft.Json.JsonConvert.SerializeObject(dataPaging);

                dynamic Paging =
                new
                {
                    lastpage = dataPaging.PageCount,
                    curentpage = search.Page,
                };

                _memory.Remove(ConstantStrings.CachePromotionName);
                _memory.Remove(ConstantStrings.CacheProductOptionName);

                return new { Data = rs, Paging = Paging };
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public bool Enable(long[] Pid, bool active)
        {
            try
            {
                foreach (var item in Pid)
                {
                    try
                    {
                        var model = _dbContext.ProductDetails.Where(p => p.Pid == item).FirstOrDefault();
                        model.Enabled = active;
                        _dbContext.SaveChanges();

                    }
                    catch (Exception ex)
                    {

                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public dynamic Delete(int Pid)
        {
            try
            {
                var orderList = _dbContext.Orders.Where(p => p.ProductDetailPid == Pid).FirstOrDefault();
                if(orderList != null)
                {
                    return new { isError = false, messError = "Có đơn hàng đang sử dụng sản phẩm này!" };
                }
                var model = _dbContext.ProductDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                model.Deleted = true;
                model.UpdateDate = DateTime.Now;
                var deleteImage = _dbContext.Images_Products.Where(p => p.ProductDetailPid == model.Pid).ToList();
                foreach (var item2 in deleteImage)
                {
                    _fileServices.DeleteFile(UrlProductImages, item2.Images);
                    _fileServices.DeleteFile(UrlProductImages, Fullmages + item2.Images);
                }
                _dbContext.Images_Products.RemoveRange(deleteImage);

                _fileServices.DeleteFile(UrlProductImages, model.PicThumb);
                _fileServices.DeleteFile(UrlProductImages, Fullmages + model.PicThumb);

                _dbContext.SaveChanges();

                dynamic logObj = new ExpandoObject();
                logObj.Title = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                logObj.Pid = model.Pid;
                logObj.Cate = ProductId;
                _common.SaveLog(1, "delete", logObj);
                return new { isError = true, messError = "" };
            }
            catch (Exception ex)
            {

                return new { isError = false, messError = "Something is wrong!" };
            }

        }
        public dynamic Delete(long[] Pid)
        {
            try
            {
                var orderList = _dbContext.Orders.Where(p => Pid.Contains(p.ProductDetailPid)).ToList();
                if(orderList.Count > 0)
                {
                    return new { isError = false, messError = "Có đơn hàng đang sử dụng những sản phẩm này!" };
                }
                foreach (var item in Pid)
                {
                    try
                    {
                        var model = _dbContext.ProductDetails.Where(p => p.Pid == item).FirstOrDefault();
                        if (model != null)
                        {
                            model.Deleted = true;
                            model.UpdateDate = DateTime.Now;

                            _fileServices.DeleteFile(UrlProductImages, model.PicThumb);
                            _fileServices.DeleteFile(UrlProductImages, Fullmages + model.PicThumb);
                            var deleteImage = _dbContext.Images_Products.Where(p => p.ProductDetailPid == model.Pid).ToList();
                            foreach (var item2 in deleteImage)
                            {
                                _fileServices.DeleteFile(UrlProductImages, item2.Images);
                                _fileServices.DeleteFile(UrlProductImages, Fullmages + item2.Images);
                            }
                            _dbContext.Images_Products.RemoveRange(deleteImage);

                            _dbContext.SaveChanges();

                            dynamic logObj = new ExpandoObject();
                            logObj.Title = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == model.Pid && p.LangKey == DefaultLang).FirstOrDefault().Title;
                            logObj.Pid = model.Pid;
                            logObj.Cate = ProductId;
                            _common.SaveLog(1, "delete", logObj);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                return new { isError = true, messError = "" };
            }
            catch (Exception ex)
            {

                return new { isError = false, messError = "Something is wrong!" };
            }
        }
        public dynamic Edit(int Pid)
        {
            try
            {

                var data = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == Pid).ToList();
                var detail = _dbContext.ProductDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                var listDataImages = _dbContext.Images_Products.Where(p => p.ProductDetailPid == Pid).ToList();
                //var productOptionOriginList = _dbContext.ProductOptions.Where(x => !x.Deleted).OrderByDescending(x => x.Order).ToList();
                //var productOptionList = _dbContext.ProductOption_ProductDetails.ToList();
                List<dynamic> productOptionReturnList = new List<dynamic>();
                //foreach (var item in productOptionOriginList)
                //{
                //    var optionPrice = productOptionList.Where(x => x.ProductOptionPid == item.Pid && x.ProductDetailPid == Pid).FirstOrDefault();
                //    dynamic d = new ExpandoObject();
                //    if (optionPrice != null)
                //    {
                //        d.optionId = item.Pid;
                //        d.code = item.Code;
                //        d.price = optionPrice.Price;
                //        d.priceDiscount = optionPrice.PriceDiscount;
                //        d.status = optionPrice.Status;
                //    }
                //    else
                //    {
                //        d.optionId = item.Pid;
                //        d.price = 0;
                //        d.code = item.Code;
                //        d.priceDiscount = 0;
                //        d.status = 1;
                //    }
                //    productOptionReturnList.Add(d);
                //}

                List<dynamic> listData = new List<dynamic>();
                List<dynamic> listImages = new List<dynamic>();
                List<dynamic> listImages_lang = new List<dynamic>();
                List<dynamic> listPrice = new List<dynamic>();
                var prices = _dbContext.ProductCate_ProductDetails.Where(p => p.ProductDetailPid == Pid).ToList();
                foreach (var item in prices)
                {
                    dynamic child = new ExpandoObject();
                    child.Pid = item.ProductCatePid;
                    child.Price = item.Price;
                    child.Enable = item.Enable;
                    listPrice.Add(child);
                }
                foreach (var item in data)
                {
                    var Product = _dbContext.ProductDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                    if (Product.Deleted == false)
                    {
                        dynamic child = new ExpandoObject();
                        child.Title = item.Title;
                        child.PicThumb = UrlProductImages + Product.PicThumb;
                        child.LangKey = item.LangKey;
                        child.Description = item.Description;
                        child.TitleSEO = item.TitleSEO;
                        child.DescriptionSEO = item.DescriptionSEO;
                        child.Content = item.Content;
                        child.Content2 = item.Content2;
                        child.Material = item.Material;
                        child.Unit = item.Unit;
                        child.ShortContent = item.ShortContent;
                        child.Pid = item.Pid;
                        child.ProductDetailPid = item.ProductDetailPid;
                        listData.Add(child);
                    }

                }
                foreach (var item in listDataImages)
                {
                    var multiLang = _dbContext.MultiLang_Images_Products.Where(p => p.ImagesProductPid == item.Pid).ToList();
                    dynamic child = new ExpandoObject();
                    child.Order = item.Order;
                    child.Images = UrlProductImages + item.Images;
                    child.Pid = item.Pid;
                    child.Status = "edit";
                    listImages.Add(child);
                    foreach (var item2 in multiLang)
                    {
                        dynamic child2 = new ExpandoObject();
                        child2.Caption = item2.Caption;
                        child2.LangKey = item2.LangKey;
                        child2.Pid = item2.Pid;
                        child2.ImagesPid = item2.ImagesProductPid;
                        listImages_lang.Add(child2);
                    }
                }
                var getListCates = _dbContext.ProductCate_ProductDetails.Where(x => x.ProductDetailPid == Pid).Select(x => x.ProductCatePid).ToList();
                List<dynamic> listCates = new List<dynamic>();
                if (getListCates!.Any())
                {
                    foreach (var item in getListCates)
                    {
                        listCates.Add(item);
                    }
                }

                //var getListColors = _dbContext.ProductColor_ProductDetails.Where(x => x.ProductDetailPid == Pid).Select(x => x.ProductColorPid).ToList();
                List<dynamic> listColors = new List<dynamic>();
                //if (getListColors!.Any())
                //{
                //    foreach (var item in getListColors)
                //    {
                //        listColors.Add(item);
                //    }
                //}

                return new
                {
                    detail = new
                    {
                        detail.TagKey,
                        detail.Enabled,
                        detail.Pid,
                        detail.Code,
                        detail.Size,
                        detail.Tiki,
                        detail.Lazada,
                        detail.Shopee,
                        detail.Stock,
                        detail.Price,
                        detail.PriceDiscount,
                        detail.Level,
                        detail.UserAmount,
                        detail.Cycle,
                        PicThumb = UrlProductImages + detail.PicThumb
                    },
                    list = listData,
                    listCates = listCates,
                    listColors = listColors,
                    productOptionList = productOptionReturnList,
                    images = listImages,
                    images_lang = listImages_lang,
                    listPrice = listPrice

                };

            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public dynamic Insert(ProductDetail productDetail, List<MultiLang_ProductDetail> multiLangProductDetail,
                            IFormFile PicThumb, List<Temp_Images> listImagesProduct,
                            List<Temp_MultiLang_Images> listLangImagesProduct, string listCates, string productOptionList, string listColors, string listTypes, List<ProductPrice> listProductPrice)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var defaultData = multiLangProductDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault();
                    string nameImages = multiLangProductDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;

                    if (!string.IsNullOrEmpty(productDetail.TagKey))
                    {
                        var arrTagKey = productDetail.TagKey.Split(",");
                        var tagkeys = new List<string>();
                        foreach (var item in arrTagKey)
                        {
                            tagkeys.Add(_common.EncodeTitle(item.Replace("#", "")));
                        }
                        productDetail.SlugTagKey = string.Join(",", tagkeys);
                    }

                    int maxOrder = _dbContext.ProductDetails.Max(x => (int?)x.Order) ?? 1;
                    productDetail.Order = maxOrder + 1;

                    if (string.IsNullOrEmpty(productDetail.Code))
                    {
                        if (!string.IsNullOrEmpty(_productCode))
                        {
                            var code = _dbContext.ProductDetails.Where(x => x.Code.Contains(_productCode)).Select(x => x.Code).ToList();
                            if (!code.Any())
                            {
                                productDetail.Code = _productCode + 1;
                            }
                            else
                            {
                                var listInt = new List<int>();
                                foreach (var item in code)
                                {
                                    var str = item.Split(_productCode)[1];
                                    var tem = Convert.ToInt32(str);
                                    listInt.Add(tem);
                                }
                                productDetail.Code = _productCode + (listInt.Max() + 1);
                            }
                        }
                    }
                    var levelExisted = _dbContext.ProductDetails.Where(p => p.Level == productDetail.Level).FirstOrDefault();
                    if(levelExisted != null)
                    {
                        transaction.Rollback();
                        return new { status = false, mess = "Level đã tồn tại" };
                    }

                    _dbContext.ProductDetails.Add(productDetail);
                    _dbContext.SaveChanges();
                    DateTime ts = DateTime.Now;
                    ts = new DateTime(ts.Year, ts.Month, ts.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    #region save cate
                    string[] cateArray = { RootProductCatePid.ToString() };
                    //string[] colorArray = { RootProductCatePid.ToString() };
                    //string[] typeArray = { RootProductCatePid.ToString() };
                    if (listCates != null)
                    {
                        cateArray = listCates.Split(',');
                    }
                    foreach (var item in cateArray)
                    {
                        ProductCate_ProductDetail ProductCate_ProductDetail = new ProductCate_ProductDetail();
                        ProductCate_ProductDetail.ProductCatePid = Convert.ToInt32(item);
                        ProductCate_ProductDetail.ProductDetailPid = productDetail.Pid;
                        _dbContext.ProductCate_ProductDetails.Add(ProductCate_ProductDetail);
                    }
                    _dbContext.SaveChanges();

                    //if (listColors != null)
                    //{
                    //    colorArray = listColors.Split(',');
                    //}

                    //foreach (var item in colorArray)
                    //{
                    //    ProductColor_ProductDetail ProductColor_ProductDetail = new ProductColor_ProductDetail();
                    //    ProductColor_ProductDetail.ProductColorPid = Convert.ToInt32(item);
                    //    ProductColor_ProductDetail.ProductDetailPid = ProductDetail.Pid;
                    //    _dbContext.ProductColor_ProductDetails.Add(ProductColor_ProductDetail);
                    //}
                    //_dbContext.SaveChanges();

                    // _dbContext.SaveChanges();

                    #endregion
                    #region save list price
                    if (listProductPrice.Any())
                    {
                        foreach (var item in listProductPrice)
                        {
                            ProductCate_ProductDetail productCate_ProductDetail = new ProductCate_ProductDetail();
                            productCate_ProductDetail.Price = item.Price;
                            productCate_ProductDetail.Enable = item.Enable;
                            productCate_ProductDetail.ProductCatePid = item.Pid;
                            productCate_ProductDetail.ProductDetailPid = productDetail.Pid;
                            _dbContext.ProductCate_ProductDetails.Add(productCate_ProductDetail);
                        }
                        _dbContext.SaveChanges();
                    }
                    #endregion
                    #region save multi lang
                    foreach (var item in multiLangProductDetail)
                    {
                        string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                        item.ProductDetailPid = productDetail.Pid;
                        if (item.LangKey != DefaultLang)
                        {
                            item.Title = title;
                            item.Description = string.IsNullOrEmpty(item.Description) ? _common.RemoveHtmlTag(defaultData.Description) : _common.RemoveHtmlTag(item.Description);
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;
                            item.Content2 = string.IsNullOrEmpty(item.Content2) ? defaultData.Content2 : item.Content2;
                            item.Material = string.IsNullOrEmpty(item.Material) ? defaultData.Material : item.Material;
                            item.Unit = string.IsNullOrEmpty(item.Unit) ? defaultData.Unit : item.Unit;
                            item.ShortContent = string.IsNullOrEmpty(item.ShortContent) ? defaultData.ShortContent : item.ShortContent;
                        }

                        #region check exist slug
                        var newSlug = _common.EncodeTitle(title);
                        var existSlug = (from a in _dbContext.ProductDetails
                                         join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                         where (b.LangKey == item.LangKey && b.Slug == newSlug)
                                         select a).FirstOrDefault();

                        if (existSlug != null)
                        {
                            transaction.Rollback();
                            return new { status = false, mess = "Tiêu đề đã tồn tại" };
                        }
                        else
                        {
                            item.Slug = newSlug;
                        }
                        #endregion

                        item.TitleWithoutSign = _common.RemoveSign4VietnameseString(title);
                        _dbContext.MultiLang_ProductDetails.Add(item);
                        _dbContext.SaveChanges();
                    }
                    #endregion
                    #region Images
                    if (PicThumb != null)
                    {
                        if (WatermarkActive == "on")
                        {
                            dynamic saveFileStatus = _fileServices.SaveFileWithWatermark(PicThumb, UrlProductImages, nameImages + "-" + productDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(PicThumb, UrlProductImages, saveFileStatus.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(PicThumb, UrlProductImages, saveFileStatus.fileName);
                                }
                                productDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlProductImages, nameImages + "-" + productDetail.Pid);
                            if (!saveFileStatus.isError)
                            {
                                _fileServices.ResizeThumbImage(PicThumb, UrlProductImages, saveFileStatus.fileName);
                                productDetail.PicThumb = saveFileStatus.fileName;
                                _dbContext.SaveChanges();
                            }
                        }
                    }

                    int demImages = new Random().Next(9000, 10000);
                    if (listImagesProduct != null)
                    {
                        foreach (var item in listImagesProduct)
                        {
                            demImages = demImages + 1;
                            dynamic temp = new ExpandoObject();
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlProductImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlProductImages, _common.EncodeTitle(defaultData.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_Product Images_Products = new Images_Product();

                                Images_Products.Images = temp.fileName;
                                Images_Products.Order = item.Order;
                                Images_Products.ProductDetailPid = productDetail.Pid;
                                _dbContext.AddRange(Images_Products);
                                _dbContext.SaveChanges();

                            }
                        }
                    }
                    #endregion


                    //var proOption = JsonConvert.DeserializeObject<List<ProductOptionDto>>(productOptionList);
                    //if (proOption.Any())
                    //{
                    //    foreach (var item in proOption)
                    //    {
                    //        ProductOption_ProductDetail productOptionDetail = new ProductOption_ProductDetail();
                    //        productOptionDetail.ProductOptionPid = item.OptionId;
                    //        productOptionDetail.Price = item.Price;
                    //        productOptionDetail.Status = item.Status;
                    //        productOptionDetail.PriceDiscount = item.PriceDiscount;
                    //        productOptionDetail.ProductDetailPid = ProductDetail.Pid;
                    //        _dbContext.ProductOption_ProductDetails.Add(productOptionDetail);
                    //        _dbContext.SaveChanges();
                    //    }
                    //}

                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = defaultData.Title;
                    logObj.Pid = productDetail.Pid;
                    logObj.Cate = ProductId;
                    _common.SaveLog(1, "new", logObj);
                    #endregion
                    transaction.Commit();

                    return new { status = true, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _common.SaveLogError(ex);
                    messErr = "Something Wrong!";

                    return new { status = false, mess = messErr };
                }
            }
        }
        public dynamic Update(ProductDetail productDetail, List<MultiLang_ProductDetail> multiLangProductDetail, IFormFile Images, List<Temp_Images> listDeleteImages,
                        List<Temp_Images> listImagesProduct, List<Temp_MultiLang_Images> listLangImagesProduct, string listCates, string productOptionList, string listColors, string listTypes, List<ProductPrice> listProductPrice)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                string messErr = "";

                try
                {
                    var model = _dbContext.ProductDetails.Where(p => p.Pid == productDetail.Pid).FirstOrDefault();
                    var nameImages = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == productDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                    model.Enabled = productDetail.Enabled;
                    model.TagKey = productDetail.TagKey;
                    model.Code = productDetail.Code;
                    model.Size = productDetail.Size;
                    model.Tiki = productDetail.Tiki;
                    model.Lazada = productDetail.Lazada;
                    model.Shopee = productDetail.Shopee;
                    model.Stock = productDetail.Stock;
                    model.Price = productDetail.Price;
                    model.PriceDiscount = productDetail.PriceDiscount;
                    model.Level = productDetail.Level;
                    model.UserAmount = productDetail.UserAmount;
                    model.Cycle = productDetail.Cycle;

                    if (string.IsNullOrEmpty(productDetail.Code))
                    {
                        if (!string.IsNullOrEmpty(_productCode))
                        {
                            var code = _dbContext.ProductDetails.Where(x => x.Code.Contains(_productCode)).Select(x => x.Code).ToList();
                            if (!code.Any())
                            {
                                model.Code = _productCode + 1;
                            }
                            else
                            {
                                var listInt = new List<int>();
                                foreach (var item in code)
                                {
                                    var str = item.Split(_productCode)[1];
                                    var tem = Convert.ToInt32(str);
                                    listInt.Add(tem);
                                }
                                model.Code = _productCode + (listInt.Max() + 1);
                            }
                        }
                    }


                    model.UpdateDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(productDetail.TagKey))
                    {
                        var arrTagKey = productDetail.TagKey.Split(",");
                        var tagkeys = new List<string>();
                        foreach (var item in arrTagKey)
                        {
                            tagkeys.Add(_common.EncodeTitle(item.Replace("#", "")));
                        }
                        model.SlugTagKey = string.Join(",", tagkeys);
                    }

                    var levelExisted = _dbContext.ProductDetails.Where(p => p.Level == model.Level && p.Pid != model.Pid).FirstOrDefault();
                    if (levelExisted != null)
                    {
                        transaction.Rollback();
                        return new { status = false, mess = "Level đã tồn tại" };
                    }
                    #region edit multi_lang
                    foreach (var item in multiLangProductDetail)
                    {
                        var multiModel = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == productDetail.Pid && p.LangKey == item.LangKey).FirstOrDefault();

                        if (multiModel != null)
                        {
                            #region check exist slug
                            string newSlug = _common.EncodeTitle(item.Title);
                            var existSlug = (from a in _dbContext.ProductDetails
                                             join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                             where (b.LangKey == item.LangKey && b.Slug == newSlug && a.Pid != model.Pid)
                                             select a).FirstOrDefault();

                            if (existSlug != null)
                            {
                                transaction.Rollback();
                                return new { status = false, mess = "Tiêu đề đã tồn tại" };
                            }
                            else
                            {
                                multiModel.Slug = newSlug;
                            }
                            #endregion
                            multiModel.TitleWithoutSign = _common.RemoveSign4VietnameseString(item.Title);
                            multiModel.Title = item.Title;
                            multiModel.Content = item.Content;
                            multiModel.Content2 = item.Content2;
                            multiModel.Material = item.Material;
                            multiModel.Unit = item.Unit;
                            multiModel.ShortContent = item.ShortContent;
                            multiModel.Description = _common.RemoveHtmlTag(item.Description);
                            multiModel.DescriptionSEO = _common.RemoveHtmlTag(item.DescriptionSEO);
                            multiModel.TitleSEO = _common.RemoveHtmlTag(item.TitleSEO);
                        }
                        else
                        {
                            var defaultData = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == productDetail.Pid && p.LangKey == DefaultLang).FirstOrDefault();
                            string title = string.IsNullOrEmpty(item.Title) ? defaultData.Title : item.Title;
                            item.Title = title;
                            item.ProductDetailPid = productDetail.Pid;
                            item.Description = string.IsNullOrEmpty(item.Description) ? _common.RemoveHtmlTag(defaultData.Description) : _common.RemoveHtmlTag(item.Description);
                            item.Content = string.IsNullOrEmpty(item.Content) ? defaultData.Content : item.Content;
                            item.Content2 = string.IsNullOrEmpty(item.Content2) ? defaultData.Content2 : item.Content2;
                            item.Material = string.IsNullOrEmpty(item.Material) ? defaultData.Material : item.Material;
                            item.Unit = string.IsNullOrEmpty(item.Unit) ? defaultData.Unit : item.Unit;
                            item.ShortContent = string.IsNullOrEmpty(item.ShortContent) ? defaultData.ShortContent : item.ShortContent;

                            #region check exist slug
                            string newSlug = _common.EncodeTitle(title);
                            var existSlug = (from a in _dbContext.ProductDetails
                                             join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                             where (b.LangKey == item.LangKey && b.Slug == newSlug && a.Pid != model.Pid)
                                             select a).FirstOrDefault();

                            if (existSlug != null)
                            {
                                transaction.Rollback();
                                return new { status = false, mess = "Tiêu đề đã tồn tại" };
                            }
                            else
                            {
                                item.Slug = newSlug;
                            }
                            #endregion

                            item.TitleWithoutSign = _common.RemoveSign4VietnameseString(title);
                            item.DescriptionSEO = _common.RemoveHtmlTag(item.DescriptionSEO);
                            item.TitleSEO = _common.RemoveHtmlTag(item.TitleSEO);
                            _dbContext.MultiLang_ProductDetails.Add(item);
                        }
                        _dbContext.SaveChanges();

                    }
                    #endregion
                    #region edit picthumb
                    if (Images != null)
                    {
                        _fileServices.DeleteFile(UrlProductImages, model.PicThumb);
                        _fileServices.DeleteFile(UrlProductImages, Fullmages + model.PicThumb);

                        if (WatermarkActive == "on")
                        {
                            dynamic kt = _fileServices.SaveFileWithWatermark(Images, UrlProductImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                if (WatermarkPicThumbActive == "on")
                                {
                                    _fileServices.ResizeThumbImageWithWatermark(Images, UrlProductImages, kt.fileName);
                                }
                                else
                                {
                                    _fileServices.ResizeThumbImage(Images, UrlProductImages, kt.fileName);
                                }
                                model.PicThumb = kt.fileName;
                            }
                        }
                        else
                        {
                            dynamic kt = _fileServices.SaveFile(Images, UrlProductImages, nameImages.Title);
                            if (!kt.isError)
                            {
                                _fileServices.ResizeThumbImage(Images, UrlProductImages, kt.fileName);
                                model.PicThumb = kt.fileName;
                            }
                        }
                    }

                    #endregion

                    #region edit Cate
                    var listCateOld = _dbContext.ProductCate_ProductDetails.Where(x => x.ProductDetailPid == productDetail.Pid).ToList();
                    _dbContext.ProductCate_ProductDetails.RemoveRange(listCateOld);

                    var listColorOld = _dbContext.ProductColor_ProductDetails.Where(x => x.ProductDetailPid == productDetail.Pid).ToList();
                    _dbContext.ProductColor_ProductDetails.RemoveRange(listColorOld);

                    _dbContext.SaveChanges();
                    string[] cateArray = { RootProductCatePid.ToString() };
                    //string[] colorArray = { RootProductCatePid.ToString() };
                    //string[] typeArray = { RootProductCatePid.ToString() };
                    if (listCates != null)
                    {
                        cateArray = listCates.Split(',');
                    }
                    //if (listColors != null)
                    //{
                    //    colorArray = listColors.Split(',');
                    //}
                    //if (listTypes != null)
                    //{
                    //    typeArray = listTypes.Split(',');
                    //}
                    foreach (var item in cateArray)
                    {
                        ProductCate_ProductDetail ProductCate_ProductDetail = new ProductCate_ProductDetail();
                        ProductCate_ProductDetail.ProductCatePid = Convert.ToInt32(item);
                        ProductCate_ProductDetail.ProductDetailPid = productDetail.Pid;
                        _dbContext.ProductCate_ProductDetails.Add(ProductCate_ProductDetail);
                    }
                    _dbContext.SaveChanges();

                    //foreach (var item in colorArray)
                    //{
                    //    ProductColor_ProductDetail ProductColor_ProductDetail = new ProductColor_ProductDetail();
                    //    ProductColor_ProductDetail.ProductColorPid = Convert.ToInt32(item);
                    //    ProductColor_ProductDetail.ProductDetailPid = ProductDetail.Pid;
                    //    _dbContext.ProductColor_ProductDetails.Add(ProductColor_ProductDetail);
                    //}
                    //_dbContext.SaveChanges();

                    #endregion

                    #region edit price
                    var listPrices = _dbContext.ProductCate_ProductDetails.Where(x => x.ProductDetailPid == model.Pid).ToList();
                    _dbContext.ProductCate_ProductDetails.RemoveRange(listPrices);
                    if (listProductPrice.Any())
                    {
                        foreach (var item in listProductPrice)
                        {
                            ProductCate_ProductDetail productCate_ProductDetail = new ProductCate_ProductDetail();
                            productCate_ProductDetail.Price = item.Price;
                            productCate_ProductDetail.Enable = item.Enable;
                            productCate_ProductDetail.ProductCatePid = item.Pid;
                            productCate_ProductDetail.ProductDetailPid = model.Pid;
                            _dbContext.ProductCate_ProductDetails.Add(productCate_ProductDetail);
                        }
                        _dbContext.SaveChanges();
                    }
                    #endregion
                    #region save list images
                    //delete images

                    //var dataDeleteImages = _dbContext.Images_Products.Where(p => p.ProductDetailPid == ProductDetail.Pid).ToList();
                    foreach (var item in listDeleteImages)
                    {
                        try
                        {
                            var deleteImages = _dbContext.Images_Products.Where(p => p.Pid == Convert.ToInt32(item.Pid)).FirstOrDefault();
                            _fileServices.DeleteFile(UrlProductImages, deleteImages.Images);
                            _fileServices.DeleteFile(UrlProductImages, Fullmages + deleteImages.Images);
                            _dbContext.Remove(deleteImages);
                            _dbContext.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    //save list images
                    int demImages = new Random().Next(9000, 10000);
                    foreach (var item in listImagesProduct)
                    {
                        demImages = demImages + 1;
                        var ktImages = item.Images.Split(',');
                        dynamic temp = new ExpandoObject();

                        if (item.Status == "new")
                        {
                            if (WatermarkActive == "on")
                            {
                                temp = _fileServices.SaveImagesBase64WithWatermark(item.Images, UrlProductImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            else
                            {
                                temp = _fileServices.SaveImagesBase64(item.Images, UrlProductImages, _common.EncodeTitle(nameImages.Title) + "-" + demImages);
                            }
                            if (temp.isError)
                            {
                                Images_Product Images_Products = new Images_Product();

                                Images_Products.Images = temp.fileName;
                                Images_Products.Order = item.Order;
                                Images_Products.ProductDetailPid = productDetail.Pid;
                                _dbContext.Images_Products.Add(Images_Products);
                                _dbContext.SaveChanges();
                                var temp_MultiLang_s = listLangImagesProduct.Where(p => p.ImagesPid == item.Pid).ToList();
                                foreach (var itemTemp_MultiLang_s in temp_MultiLang_s)
                                {
                                    MultiLang_Images_Product multiLang_Images_Products = new MultiLang_Images_Product();
                                    multiLang_Images_Products.LangKey = itemTemp_MultiLang_s.LangKey;
                                    multiLang_Images_Products.ImagesProductPid = Images_Products.Pid;
                                    multiLang_Images_Products.Caption = itemTemp_MultiLang_s.Caption;
                                    _dbContext.Add(multiLang_Images_Products);
                                    _dbContext.SaveChanges();
                                }

                            }
                        }
                        else if (item.Status == "edit")
                        {
                            foreach (var tempItem in listLangImagesProduct.Where(q => q.ImagesPid == item.Pid).ToList())
                            {
                                var tempModel = _dbContext.MultiLang_Images_Products.Where(p => p.Pid == Convert.ToInt32(tempItem.Pid)).FirstOrDefault();
                                if (tempModel != null)
                                {

                                    tempModel.Caption = tempItem.Caption;
                                }
                                else
                                {
                                    MultiLang_Images_Product multiLang_Images_Products_edit = new MultiLang_Images_Product();

                                    multiLang_Images_Products_edit.ImagesProductPid = Convert.ToInt32(item.Pid);
                                    multiLang_Images_Products_edit.Caption = tempItem.Caption;
                                    multiLang_Images_Products_edit.LangKey = tempItem.LangKey;
                                    _dbContext.MultiLang_Images_Products.Add(multiLang_Images_Products_edit);
                                }
                                _dbContext.SaveChanges();

                            }

                        }

                    }
                    #endregion
                    _dbContext.SaveChanges();



                    //var proOption = JsonConvert.DeserializeObject<List<ProductOptionDto>>(productOptionList);
                    //if (proOption.Any())
                    //{
                    //    var test = _dbContext.ProductOption_ProductDetails.Where(x => x.ProductDetailPid == model.Pid).ToList();
                    //    _dbContext.ProductOption_ProductDetails.RemoveRange(test);
                    //    foreach (var item in proOption)
                    //    {
                    //        ProductOption_ProductDetail productOptionDetail = new ProductOption_ProductDetail();
                    //        productOptionDetail.ProductOptionPid = item.OptionId;
                    //        productOptionDetail.Price = item.Price;
                    //        productOptionDetail.Status = item.Status;
                    //        productOptionDetail.PriceDiscount = item.PriceDiscount;
                    //        productOptionDetail.ProductDetailPid = model.Pid;
                    //        _dbContext.ProductOption_ProductDetails.Add(productOptionDetail);
                    //        _dbContext.SaveChanges();
                    //    }
                    //}

                    #region save log
                    dynamic logObj = new ExpandoObject();
                    logObj.Title = multiLangProductDetail.Where(p => p.LangKey == DefaultLang).FirstOrDefault().Title;
                    logObj.Pid = productDetail.Pid;
                    logObj.Cate = ProductId;
                    _common.SaveLog(1, "update", logObj);
                    #endregion
                    transaction.Commit();

                    return new { status = true, mess = messErr };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _common.SaveLogError(ex);
                    messErr = "Something Wrong!";

                    return new { status = false, mess = messErr };
                }
            }
        }
        public bool Count(int code)
        {
            try
            {
                #region check count
                var count = _dbContext.Users.Where(p => p.GroupUserCode == code).Count();
                #endregion
                if (count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
             ;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public string Search(SearchDto searchData)
        {
            try
            {
                //int page = 1;
                //int limit = 25;
                var data = _dbContext.GroupUsers.Where(p => ((
                                                           ((p.Name.Contains(searchData.Key) ||
                                                           p.Role.Contains(searchData.Key)) || String.IsNullOrEmpty(searchData.Key))
                                                           ) && (p.Enabled.Equals(searchData.Enable) || searchData.Enable.Equals(null))) &&

                                                    p.Deleted == false).ToList();
                //PagedList<GroupUser> pagingData = new PagedList<GroupUser>(data, page, limit);
                List<dynamic> listData = new List<dynamic>();
                foreach (var item in data)
                {
                    dynamic child = new ExpandoObject();
                    child.Role = item.Role;
                    child.Code = item.Code;
                    child.Name = item.Name;
                    child.Enabled = item.Enabled;
                    child.CountUser = _dbContext.Users.Where(p => p.GroupUserCode == item.Code && p.Deleted == false).Count();
                    listData.Add(child);
                }
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(listData);

                return result;
            }
            catch (Exception ex)
            {

                return "[]";
            }
        }
        public bool Coppy(long[] Pid)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in Pid)
                    {
                        try
                        {
                            var model = _dbContext.ProductDetails.Where(p => p.Pid == item).FirstOrDefault();
                            int maxOrder = _dbContext.ProductDetails.Max(x => (int?)x.Order) ?? 1;

                            ProductDetail addProduct = new ProductDetail();
                            addProduct.Order = maxOrder + 1;
                            addProduct.PicThumb = "";
                            addProduct.Enabled = false;
                            addProduct.TagKey = model.TagKey;
                            addProduct.Stock = model.Stock;
                            addProduct.Size = model.Size;
                            addProduct.Price = model.Price;
                            addProduct.PriceDiscount = model.PriceDiscount;
                            _dbContext.ProductDetails.Add(addProduct);
                            _dbContext.SaveChanges();

                            var productCate_ProductDetail = new ProductCate_ProductDetail();
                            productCate_ProductDetail.ProductCatePid = ConstantStrings.RootProductCatePid;
                            productCate_ProductDetail.ProductDetailPid = addProduct.Pid;
                            _dbContext.ProductCate_ProductDetails.Add(productCate_ProductDetail);
                            _dbContext.SaveChanges();

                            List<MultiLang_ProductDetail> detailModel = _dbContext.MultiLang_ProductDetails.Where(p => p.ProductDetailPid == model.Pid).ToList();
                            foreach (MultiLang_ProductDetail itemDetail in detailModel)
                            {
                                var coppyCount = _dbContext.MultiLang_ProductDetails.Where(p => p.Title.Contains(itemDetail.Title)).ToList().Count() + 1;
                                MultiLang_ProductDetail temp = new MultiLang_ProductDetail();
                                temp.Content = itemDetail.Content;
                                temp.Description = itemDetail.Description;
                                temp.LangKey = itemDetail.LangKey;
                                temp.Content2 = itemDetail.Content2;
                                temp.Material = itemDetail.Material;
                                temp.Unit = itemDetail.Unit;
                                temp.Title = itemDetail.Title + " (Coppy " + coppyCount.ToString() + ")";
                                temp.Slug = _common.EncodeTitle(temp.Title);
                                temp.ProductDetailPid = addProduct.Pid;
                                _dbContext.MultiLang_ProductDetails.Add(temp);
                                _dbContext.SaveChanges();
                            }

                            //_dbContext.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            //transaction.Rollback();
                        }

                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public dynamic Validation(dynamic data)
        {
            try
            {

                return new { error = true, errorMess = "Lỗi không xác định" };
            }
            catch (Exception ex)
            {

                return new { error = true, errorMess = "Lỗi không xác định" };
            }
        }
        public bool Preview(string obj, string objDetail, IFormFile PicThumb)
        {
            try
            {
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == ProductId.ToString()).FirstOrDefault();
                if (model != null)
                {
                    model.Obj = obj;
                    model.ObjDetail = objDetail;
                    model.IsEditPicThumb = false;
                    if (PicThumb != null)
                    {
                        dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlPreviewImages, "book" + "-" + model.Pid);

                        if (!saveFileStatus.isError)
                        {
                            model.IsEditPicThumb = true;
                            _fileServices.ResizeThumbImage(PicThumb, UrlPreviewImages, saveFileStatus.fileName);
                            model.PicThumb = saveFileStatus.fileName;
                            _dbContext.SaveChanges();
                        }
                    }
                    _dbContext.SaveChanges();
                }
                else
                {
                    ModulePreview modulePreview = new ModulePreview();
                    modulePreview.Obj = obj;
                    modulePreview.ObjDetail = objDetail;
                    modulePreview.ModuleId = ProductId.ToString();
                    modulePreview.IsEditPicThumb = true;
                    _dbContext.ModulePreviews.Add(modulePreview);
                    _dbContext.SaveChanges();
                    if (PicThumb != null)
                    {
                        dynamic saveFileStatus = _fileServices.SaveFile(PicThumb, UrlPreviewImages, "book" + "-" + modulePreview.Pid);

                        if (!saveFileStatus.isError)
                        {
                            _fileServices.ResizeThumbImage(PicThumb, UrlPreviewImages, saveFileStatus.fileName);
                            modulePreview.PicThumb = saveFileStatus.fileName;
                            _dbContext.SaveChanges();
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool MoveRow(long from, long to)
        {
            try
            {
                var toRow = _dbContext.ProductDetails.Where(p => p.Pid == to).FirstOrDefault();
                var fromRow = _dbContext.ProductDetails.Where(p => p.Pid == from).FirstOrDefault();
                var max = _dbContext.ProductDetails.Where(p => p.Deleted == false).Max(p => p.Order);
                var orderTo = toRow.Order;
                var orderFrom = fromRow.Order;
                if (fromRow.Order < toRow.Order)
                {
                    if (orderTo != max)
                    {
                        var listData = _dbContext.ProductDetails.Where(p => p.Order < toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
                        foreach (var item in listData)
                        {
                            if (item.Order > 1)
                            {
                                item.Order = item.Order - 1;

                            }
                        }
                        fromRow.Order = orderTo;
                        toRow.Order = orderTo - 1;
                    }
                    else
                    {
                        fromRow.Order = orderTo;
                        toRow.Order = orderFrom;
                    }
                }
                else
                {
                    var listData = _dbContext.ProductDetails.Where(p => p.Order > toRow.Order && p.Deleted == false && p.Pid != fromRow.Pid).ToList();
                    if (orderTo != 1)
                    {
                        foreach (var item in listData)
                        {
                            if (item.Order < max)
                            {
                                item.Order = item.Order + 1;
                            }
                        }
                        fromRow.Order = orderTo;

                        toRow.Order = orderTo + 1;
                    }
                    else
                    {
                        fromRow.Order = orderTo;
                        toRow.Order = orderFrom;
                    }

                }


                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool UpdateOrder(long Pid, int order)
        {
            try
            {
                var data = _dbContext.ProductDetails.Where(p => p.Pid == Pid).FirstOrDefault();
                data.Order = order;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public string GetProductOption()
        {
            try
            {
                return JsonConvert.SerializeObject(_dbContext.ProductOptions.Where(x => !x.Deleted).OrderByDescending(x => x.Order).ToList());

            }
            catch
            {
                return "";
            }
        }
        public bool SaveStatus(long pid, bool value, string type)
        {
            try
            {
                var data = _dbContext.ProductDetails.Where(p => p.Pid == pid).FirstOrDefault();
                if (data != null)
                {
                    if (type == "hot")
                    {
                        data.IsHot = value;
                    }
                    else
                    {
                        data.IsNew = value;
                    }

                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public string LoadProductCode()
        {
            if (!string.IsNullOrEmpty(_productCode))
            {
                var code = _dbContext.ProductDetails.Where(x => x.Code.Contains(_productCode)).Select(x => x.Code).ToList();
                if (!code.Any())
                {
                    return _productCode + 1;
                }
                else
                {
                    var listInt = new List<int>();
                    foreach (var item in code)
                    {
                        var str = item.Split(_productCode)[1];
                        var tem = Convert.ToInt32(str);
                        listInt.Add(tem);
                    }
                    return _productCode + (listInt.Max() + 1);
                }
            }
            return "";
        }
        public dynamic GetProductType(string lang)
        {
            try
            {
                var data_list = new List<dynamic>();
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var list_cate = _dbContext.ProductCates.Where(x => x.Deleted == false && x.Enabled == true && !x.isLocked).OrderBy(x => x.Order).ToList();
                foreach (var item in list_cate)
                {
                    var cate_detail = _dbContext.MultiLang_ProductCates.Where(x => x.ProductCatePid == item.Pid && x.LangKey == lang).FirstOrDefault();
                    dynamic d = new ExpandoObject();
                    d.Name = cate_detail.Name;
                    d.Pid = item.Pid;
                    d.Price = 0;
                    d.Enable = true;
                    data_list.Add(d);
                }
                return data_list;
            }
            catch
            {
                return "[]";
            }
        }
    }
}
