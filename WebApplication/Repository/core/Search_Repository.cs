using System.Collections.Generic;
using System.Linq;
using X.PagedList;
using DTO.News;
using DTO;
using System.Threading.Tasks;
using DTO.Product;
using CMS.Services.CommonServices;
using System;
using DTO.SearchResult;
using CMS.Services.TranslateServices;

namespace CMS.Repository
{
    public class Search_Repository : ISearch_Repository
    {
        private readonly DBContext _dbContext;
        private readonly ICommonServices _common;
        private readonly ITranslateServices _translate;
        private string UrlProductImages = ConstantStrings.UrlProductImages;
        private string UrlNewsImages = ConstantStrings.UrlNewsImages;
        private string UrlGalleryImages = ConstantStrings.UrlGalleryImages;
        private string UrlFeatureImages = ConstantStrings.UrlFeatureImages;
        private string Thumb = ConstantStrings.Thumb;
        private string Fullmages = ConstantStrings.Fullmages;
        public Search_Repository(DBContext dbContext, ICommonServices common, ITranslateServices translate)
        {
            _dbContext = dbContext;
            _common = common;
            _translate = translate;
        }

        public async Task<List<SearchResultDto>> GetSearchList(string lang, string key)
        {
            try
            {
                var keyword = "";

                if (!string.IsNullOrEmpty(key))
                {
                    keyword = key.ToLower();
                }

                string newsCate = _translate.GetString("menu.news");
                string serviceCate = _translate.GetString("menu.feature");
                string galleryCate = _translate.GetString("menu.gallery");

                string newsUrl = _translate.GetUrl("url.news");
                string serviceUrl = _translate.GetUrl("url.feature");
                string galleryUrl = _translate.GetUrl("url.gallery");

                var service = await (from a in _dbContext.FeatureDetails
                                     join b in _dbContext.MultiLang_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                     where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                     && (b.Title.ToLower().Contains(keyword) || b.TitleWithoutSign.Contains(keyword))
                                     orderby a.Order descending
                                     select new SearchResultDto
                                     {
                                         Pid = a.Pid,
                                         Title = b.Title,
                                         TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                         Slug = b.Slug,
                                         Description = b.Description,
                                         PicThumb = UrlFeatureImages + Thumb + a.PicThumb,
                                         CateName = serviceCate,
                                         CateSlug = serviceUrl,
                                     }).ToListAsync();

                var gallery = await (from a in _dbContext.GalleryDetails
                                     join b in _dbContext.MultiLang_GalleryDetails on a.Pid equals b.GalleryDetailPid
                                     where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                     && (b.Title.ToLower().Contains(keyword) || b.TitleWithoutSign.Contains(keyword))
                                     orderby a.Order descending
                                     select new SearchResultDto
                                     {
                                         Pid = a.Pid,
                                         Title = b.Title,
                                         TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                         Slug = b.Slug,
                                         Description = b.Description,
                                         PicThumb = UrlGalleryImages + Thumb + a.PicThumb,
                                         CateName = galleryCate,
                                         CateSlug = galleryUrl,
                                     }).ToListAsync();


                var news = await (from a in _dbContext.NewsDetails
                                  join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                                  where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                  && (b.Title.ToLower().Contains(keyword) || b.TitleWithoutSign.Contains(keyword))
                                  orderby a.Order descending
                                  select new SearchResultDto
                                  {
                                      Pid = a.Pid,
                                      Title = b.Title,
                                      TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                      Slug = b.Slug,
                                      Description = b.Description,
                                      PicThumb = UrlNewsImages + Thumb + a.PicThumb,
                                      CateName = newsCate,
                                      CateSlug = newsUrl,
                                  }).ToListAsync();
                return service.Concat(gallery).Concat(news).ToList();
            }
            catch
            {
                return new List<SearchResultDto>();
            }
        }
        public async Task<List<SearchResultDto>> GetTagkeyList(string lang, string key)
        {
            try
            {
                var keyword = "";

                if (!string.IsNullOrEmpty(key))
                {
                    keyword = _common.EncodeTitle(key);
                }

                string galleryCate = _translate.GetString("menu.gallery");

                string galleryUrl = _translate.GetUrl("url.gallery");



                var gallery = await (from a in _dbContext.GalleryDetails
                                     join b in _dbContext.MultiLang_GalleryDetails on a.Pid equals b.GalleryDetailPid
                                     where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                     && (a.SlugTagKey.Contains(keyword))
                                     orderby a.Order descending
                                     select new SearchResultDto
                                     {
                                         Pid = a.Pid,
                                         Title = b.Title,
                                         Slug = b.Slug,
                                         Description = b.Description,
                                         PicThumb = UrlGalleryImages + Thumb + a.PicThumb,
                                         CateName = galleryCate,
                                         CateSlug = galleryUrl,
                                     }).ToListAsync();

                return gallery;
            }
            catch
            {
                return new List<SearchResultDto>();
            }
        }
        public async Task<List<NewsDto>> GetNewsList(string lang, string key)
        {
            try
            {
                var keyword = "";
                if (!string.IsNullOrEmpty(key))
                {
                    keyword = key.ToLower();
                }
                var model = await (from a in _dbContext.NewsDetails
                                   join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   && (b.Title.ToLower().Contains(keyword) || b.TitleWithoutSign.Contains(keyword))
                                   orderby a.Order descending
                                   select new NewsDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Slug = b.Slug,
                                       Description = b.Description,
                                       PicThumb = UrlNewsImages + Thumb + a.PicThumb,
                                       PicFull = UrlNewsImages + Fullmages + a.PicThumb,
                                   }).ToListAsync();
                return model;
            }
            catch
            {
                return new List<NewsDto>();
            }
        }

        public async Task<List<ProductDto>> GetProductList(string lang, string key)
        {
            try
            {
                var keyword = "";
                if (!string.IsNullOrEmpty(key))
                {
                    keyword = key.ToLower();
                }
                var promotions = _common.getAllPromotions();
                var options = _common.getAllProductOptions();

                var model = await (from a in _dbContext.ProductDetails
                                   join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   && (b.Title.ToLower().Contains(keyword) || b.TitleWithoutSign.Contains(keyword))
                                   orderby a.Order descending
                                   select new ProductDto
                                   {
                                       Title = b.Title,
                                       Pid = a.Pid,
                                       Content = b.Content,
                                       Description = b.Description,
                                       Code = a.Code,
                                       Slug = b.Slug,
                                       CreateDate = a.CreateDate.Value,
                                       IsHot = a.IsHot,
                                       IsNew = a.IsNew,
                                       PicThumb = UrlProductImages + Thumb + a.PicThumb
                                   }).ToListAsync();


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
    }
}
