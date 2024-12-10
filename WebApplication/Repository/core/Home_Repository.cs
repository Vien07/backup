using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.About;
using DTO.Comment;
using DTO.FAQ;
using DTO.Gallery;
using DTO.News;
using DTO.Convenience;
using DTO.Product;
using DTO.Feature;
using DTO.Website;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Repository
{
    public class Home_Repository : IHome_Repository
    {
        private string PageLimit = "";

        private readonly ICommonServices _common;
        private readonly IMemoryCache _memoryCache;
        private readonly DBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string UrlNewsImages = ConstantStrings.UrlNewsImages;
        private string UrlGalleryImages = ConstantStrings.UrlGalleryImages;
        private string UrlFeatureImages = ConstantStrings.UrlFeatureImages;
        private string UrlCustomerImages = ConstantStrings.UrlCustomerImages;
        private string UrlHomePageImages = ConstantStrings.UrlHomePageImages;
        private string UrlProductImages = ConstantStrings.UrlProductImages;
        private string UrlProductCateImages = ConstantStrings.UrlProductCateImages;
        private string UrlProductTypeImages = ConstantStrings.UrlProductTypeImages;
        private string Fullmages = ConstantStrings.Fullmages;
        private string Thumb = ConstantStrings.Thumb;
        public Home_Repository(DBContext dbContext, ICommonServices common, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, IFileServices fileServices)
        {
            _common = common;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            PageLimit = _common.GetConfigValue(KeyPageLimit);
            _memoryCache = memoryCache;
        }

        public async Task<List<AboutDto>> GetAboutMenuList(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var model = (from a in _dbContext.AboutDetails
                             join b in _dbContext.MultiLang_AboutDetails on a.Pid equals b.AboutDetailPid
                             orderby a.Order descending
                             where (!a.Deleted && a.Enabled && a.ShowTopMenu) && (b.LangKey == lang)
                             select new AboutDto
                             {
                                 Pid = a.Pid,
                                 Title = b.Title,
                                 TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                 Slug = b.Slug
                             }).ToListAsync();
                return await model;
            }
            catch (Exception ex)
            {
                return new List<AboutDto>();
            }
        }

        public async Task<List<ConvenienceDto>> GetConvenienceList(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var model = await (from a in _dbContext.Conveniences
                                   join b in _dbContext.MultiLang_Conveniences on a.Pid equals b.ConveniencePid
                                   orderby a.Order descending
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   select new ConvenienceDto
                                   {
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Link = a.Link,
                                       PicThumb = ConstantStrings.UrlConvenienceImages + a.Url,
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                return new List<ConvenienceDto>();
            }
        }
        public async Task<List<CommentDto>> GetCommentList(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var model = await (from a in _dbContext.Comments
                                   join b in _dbContext.MultiLang_Comments on a.Pid equals b.CommentPid
                                   orderby a.Order descending
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   select new CommentDto
                                   {
                                       Title = b.Name,
                                       Description = b.Description,
                                       PicThumb = ConstantStrings.UrlCommentImages + a.PicThumb,
                                       Image = ConstantStrings.UrlCommentImages + Fullmages + a.Image,
                                       Star = a.Star
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                return new List<CommentDto>();
            }
        }
        public async Task<List<AboutDto>> GetAboutFooterList(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var model = (from a in _dbContext.AboutDetails
                             join b in _dbContext.MultiLang_AboutDetails on a.Pid equals b.AboutDetailPid
                             orderby a.Order descending
                             where (!a.Deleted && a.Enabled && a.ShowFooter) && (b.LangKey == lang)
                             select new AboutDto
                             {
                                 Pid = a.Pid,
                                 Title = b.Title,
                                 TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                 Slug = b.Slug
                             }).ToListAsync();
                return await model;
            }
            catch (Exception ex)
            {
                return new List<AboutDto>();
            }
        }
        public async Task<List<NewsDto>> GetNewsList(string lang, int limit)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var model = await (from a in _dbContext.NewsDetails
                                   join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.IsHot descending, a.Order descending
                                   select new NewsDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PicThumb = UrlNewsImages + Thumb + a.PicThumb,
                                       PicFull = UrlNewsImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch
            {
                return new List<NewsDto>();
            }
        }
        public async Task<List<FAQDto>> GetFAQList(string lang, int limit)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var model = await (from a in _dbContext.FAQDetails
                                   join b in _dbContext.MultiLang_FAQDetails on a.Pid equals b.FAQDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.IsHot descending, a.Order descending
                                   select new FAQDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Answer = b.Answer,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch
            {
                return new List<FAQDto>();
            }
        }

        public async Task<List<FeatureDto>> GetServiceList(string lang, int limit)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var model = await (from a in _dbContext.FeatureDetails
                                   join b in _dbContext.MultiLang_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.IsHot descending, a.Order descending
                                   select new FeatureDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PicThumb = UrlFeatureImages + Thumb + a.PicThumb,
                                       PicFull = UrlFeatureImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch
            {
                return new List<FeatureDto>();
            }
        }

        public async Task<List<GalleryDto>> GetGalleryList(string lang, int limit)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var model = await (from a in _dbContext.GalleryDetails
                                   join b in _dbContext.MultiLang_GalleryDetails on a.Pid equals b.GalleryDetailPid
                                   join c in _dbContext.GalleryCate_GalleryDetails on a.Pid equals c.GalleryDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.IsHot descending, a.Order descending
                                   select new GalleryDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PicThumb = UrlGalleryImages + Thumb + a.PicThumb,
                                       PicFull = UrlGalleryImages + Fullmages + a.PicThumb,
                                       CateName = _dbContext.MultiLang_GalleryCates
                                                   .Where(x => x.GalleryCatePid == c.GalleryCatePid && x.LangKey == lang)
                                                   .FirstOrDefault().Name,
                                   }).Distinct().Take(limit).ToListAsync();

                return model;
            }
            catch
            {
                return new List<GalleryDto>();
            }
        }
        public async Task<List<FeatureDto>> GetServiceListForForm(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var model = await (from a in _dbContext.FeatureDetails
                                   join b in _dbContext.MultiLang_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new FeatureDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                   }).ToListAsync();
                return model;
            }
            catch
            {
                return new List<FeatureDto>();
            }
        }
        public async Task<List<ProductDto>> GetProductList(string lang, int limit)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }

                if (limit == 0)
                {
                    limit = 100;
                }

                var promotions = _common.getAllPromotions();
                var options = _common.getAllProductOptions();

                var model = await (from a in _dbContext.ProductDetails
                                   join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   orderby a.Level
                                   select new ProductDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       PicThumb = UrlProductImages + Thumb + a.PicThumb,
                                       Slug = b.Slug,
                                       Content = b.Content,
                                       Level = a.Level,
                                       IsHot = a.IsHot,
                                       Price = 0,
                                       PriceString = "",
                                   }).OrderBy(p=> p.Level).Take(limit).ToListAsync();
                foreach(var item in model)
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
                return new List<ProductDto>();
            }
        }
        //public async Task<string> GetHomePage(string code, string lang)
        //{
        //    try
        //    {
        //        string value = "";

        //        //if (string.IsNullOrEmpty(lang))
        //        //{
        //        //    lang = DefaultLang;
        //        //}
        //        //var model = await _dbContext.HomePageIntros.Where(p => p.Code == code).FirstOrDefaultAsync();
        //        //if (model != null)
        //        //{
        //        //    var data = await _dbContext.MultiLang_HomePageIntros.Where(p => p.HomePageIntroID == model.Pid && p.LangKey == lang).FirstOrDefaultAsync();
        //        //    if (data != null)
        //        //    {
        //        //        if (code == "images")
        //        //        {
        //        //            value = UrlHomePageImages + Fullmages + model.Value;
        //        //        }
        //        //        else
        //        //        {
        //        //            value = data.Value;
        //        //        }
        //        //    }
        //        //}
        //        return value;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //}
        public async Task<List<HomePageDto>> GetHomePage(string type, string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                var model = (from a in _dbContext.HomePages
                             join b in _dbContext.MultiLang_HomePages on a.Pid equals b.HomePagePid
                             where (!a.Deleted && a.Enabled) && (b.LangKey == lang) && (a.Type == type)
                             orderby a.Order descending
                             select new HomePageDto
                             {
                                 Pid = a.Pid,
                                 Position = a.Position,
                                 //Link = a.Link,
                                 Title = b.Title,
                                 TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                 Description = b.Description,
                                 Content = b.Content,
                                 Images = UrlHomePageImages +(a.Type == "intro" ? Fullmages : "") + a.Images,
                                 IntroLink = b.IntroLink,
                             }).ToListAsync();

                return await model;
            }
            catch (Exception ex)
            {
                return new List<HomePageDto>();
            }
        }
        public async Task<List<RecruitmentCateDto>> GetRecruitmentCateList(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }
                var model = await (from a in _dbContext.RecruitmentCates
                                   join b in _dbContext.MultiLang_RecruitmentCates on a.Pid equals b.RecruitmentCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked && a.ParentId == 0) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new RecruitmentCateDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Name,
                                       TitleAlt = b.Name.Replace("\"", "").Replace("'", ""),
                                       Slug = b.Slug,
                                       Description = b.Description
                                   }).ToListAsync();
                return model;
            }
            catch (Exception ex)
            {
                return new List<RecruitmentCateDto>();
            }
        }

        public List<NewsCateDto> GetNewsCateList(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }

                if (!_memoryCache.TryGetValue(ConstantStrings.CacheNewsCateName + lang, out List<NewsCateDto> cacheValue))
                {
                    var model = (from a in _dbContext.NewsCates
                                 join b in _dbContext.MultiLang_NewsCates on a.Pid equals b.NewsCatePid
                                 where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang)
                                 orderby a.Order descending
                                 select new NewsCateDto
                                 {
                                     Pid = a.Pid,
                                     Title = b.Name,
                                     TitleAlt = b.Name.Replace("\"", "").Replace("'", ""),
                                     Slug = b.Slug,
                                     ParentId = a.ParentId,
                                     Description = b.Description
                                 }).ToList();

                    var rs = model.Where(x => x.ParentId == 0).ToList();

                    foreach (var item in rs)
                    {
                        var children = new List<NewsCateDto>();
                        recurNewsCateChildren(ref children, item.Pid, lang, model);
                        item.Children = children;
                    }


                    cacheValue = rs;

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(30));

                    _memoryCache.Set(ConstantStrings.CacheNewsCateName + lang, cacheValue, cacheEntryOptions);
                }

                return cacheValue;
            }
            catch (Exception ex)
            {
                return new List<NewsCateDto>();
            }
        }
        private void recurNewsCateChildren(ref List<NewsCateDto> children, long parentId, string lang, List<NewsCateDto> data)
        {
            var model = data.Where(x => x.ParentId == parentId).ToList();

            foreach (var item in model)
            {
                var c = new List<NewsCateDto>();
                recurNewsCateChildren(ref c, item.Pid, lang, data);
                item.Children = c;
                children.Add(item);
            }
        }

        public List<GalleryCateDto> GetGalleryCateList(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }

                if (!_memoryCache.TryGetValue(ConstantStrings.CacheGalleryCateName + lang, out List<GalleryCateDto> cacheValue))
                {
                    var model = (from a in _dbContext.GalleryCates
                                 join b in _dbContext.MultiLang_GalleryCates on a.Pid equals b.GalleryCatePid
                                 where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang)
                                 orderby a.Order descending
                                 select new GalleryCateDto
                                 {
                                     Pid = a.Pid,
                                     Title = b.Name,
                                     TitleAlt = b.Name.Replace("\"", "").Replace("'", ""),
                                     Slug = b.Slug,
                                     ParentId = a.ParentId,
                                     Description = b.Description
                                 }).ToList();

                    var rs = model.Where(x => x.ParentId == 0).ToList();

                    foreach (var item in rs)
                    {
                        var children = new List<GalleryCateDto>();
                        recurGalleryCateChildren(ref children, item.Pid, lang, model);
                        item.Children = children;
                    }


                    cacheValue = rs;

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(30));

                    _memoryCache.Set(ConstantStrings.CacheGalleryCateName + lang, cacheValue, cacheEntryOptions);
                }

                return cacheValue;
            }
            catch (Exception ex)
            {
                return new List<GalleryCateDto>();
            }
        }
        private void recurGalleryCateChildren(ref List<GalleryCateDto> children, long parentId, string lang, List<GalleryCateDto> data)
        {
            var model = data.Where(x => x.ParentId == parentId).ToList();

            foreach (var item in model)
            {
                var c = new List<GalleryCateDto>();
                recurGalleryCateChildren(ref c, item.Pid, lang, data);
                item.Children = c;
                children.Add(item);
            }
        }

        public List<ProductCateDto> GetProductCateList(string lang)
        {
            try
            {
                if (lang == null)
                {
                    lang = DefaultLang;
                }

                if (!_memoryCache.TryGetValue(ConstantStrings.CacheProductCateName + lang, out List<ProductCateDto> cacheValue))
                {
                    var model = (from a in _dbContext.ProductCates
                                 join b in _dbContext.MultiLang_ProductCates on a.Pid equals b.ProductCatePid
                                 where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang)
                                 orderby a.Order descending
                                 select new ProductCateDto
                                 {
                                     Pid = a.Pid,
                                     Title = b.Name,
                                     TitleAlt = b.Name.Replace("\"", "").Replace("'", ""),
                                     ParentId = a.ParentId,
                                     Slug = b.Slug,
                                     PicThumb = UrlProductCateImages + a.PicThumb,
                                     Description = b.Description
                                 }).ToList();

                    var rs = model.Where(x => x.ParentId == 0).ToList();

                    foreach (var item in rs)
                    {
                        var children = new List<ProductCateDto>();
                        recurProductCateChildren(ref children, item.Pid, lang, model);
                        item.Children = children;
                    }

                    cacheValue = rs;

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(30));

                    _memoryCache.Set(ConstantStrings.CacheProductCateName + lang, cacheValue, cacheEntryOptions);
                }

                return cacheValue;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return new List<ProductCateDto>();
            }
        }
        private void recurProductCateChildren(ref List<ProductCateDto> children, long parentId, string lang, List<ProductCateDto> data)
        {
            var model = data.Where(x => x.ParentId == parentId).ToList();
            foreach (var item in model)
            {
                var c = new List<ProductCateDto>();
                recurProductCateChildren(ref c, item.Pid, lang, data);
                item.Children = c;
                children.Add(item);
            }
        }
    }
}
