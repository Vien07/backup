using CMS.Areas.News.Models;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Gallery;
using DTO.News;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using static CMS.Services.ExtensionServices;

namespace CMS.Repository
{
    public class News_Repository : INews_Repository
    {
        private string DateFormat = "";
        private string PageLimit = "";

        private readonly ICommonServices _common;

        private readonly DBContext _dbContext;

        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string KeyDateFormat = ConstantStrings.KeyDateFormat;
        private string UrlNewsImages = ConstantStrings.UrlNewsImages;
        private string UrlGalleryImages = ConstantStrings.UrlGalleryImages;
        private string Thumb = ConstantStrings.Thumb;
        private string Fullmages = ConstantStrings.Fullmages;
        private string formatBase64 = ConstantStrings.formatBase64;
        private int NewsId = ConstantStrings.NewsId;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;

        public News_Repository(DBContext dbContext, ICommonServices common)
        {
            _dbContext = dbContext;
            _common = common;
            DateFormat = _common.GetConfigValue(KeyDateFormat);
            PageLimit = _common.GetConfigValue(KeyPageLimit);
        }
        public async Task<List<NewsDto>> GetList(string lang, int page)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.NewsDetails
                                   join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new NewsDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlNewsImages + Thumb + a.PicThumb,
                                       PicFull = UrlNewsImages + Fullmages + a.PicThumb,
                                   }).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<NewsDto>();
            }
        }
        public async Task<Dictionary<NewsCateDto, List<NewsDto>>> GetListDict(string lang, int limitCate, int limitList)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                Dictionary<NewsCateDto, List<NewsDto>> data = new Dictionary<NewsCateDto, List<NewsDto>>();

                var categories = await (from a in _dbContext.NewsCates
                                        join b in _dbContext.MultiLang_NewsCates on a.Pid equals b.NewsCatePid
                                        where (!a.Deleted && a.Enabled && !a.isLocked) && b.LangKey == lang
                                        orderby a.Order descending
                                        select new NewsCateDto { Pid = a.Pid, Title = b.Name, TitleAlt = b.Name.Replace("\"", "").Replace("'", ""), Slug = b.Slug }).Take(limitCate).ToListAsync();

                foreach (var item in categories)
                {

                    var model = await (from a in _dbContext.NewsDetails
                                       join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                                       join c in _dbContext.NewsCate_NewsDetails on a.Pid equals c.NewsDetailPid
                                       where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang) && c.NewsCatePid == item.Pid
                                       orderby a.Order descending
                                       select new NewsDto
                                       {
                                           Pid = a.Pid,
                                           Title = b.Title,
                                           TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                           Content = b.Content,
                                           Description = b.Description,
                                           Slug = b.Slug,
                                           PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                           PicThumb = UrlNewsImages + Thumb + a.PicThumb
                                       }).Take(limitList).ToListAsync();
                    model = model.DistinctBy(x => x.Pid).ToList();
                    if (model.Any())
                    {
                        data.Add(item, model);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                return new Dictionary<NewsCateDto, List<NewsDto>>();
            }
        }
        public async Task<List<NewsDto>> GetHighViewList(string lang, int limit)
        {
            try
            {
                if (limit == 0)
                {
                    limit = 100;
                }

                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.NewsDetails
                                   join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.IsHot descending, a.CounterView descending
                                   select new NewsDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlNewsImages + Thumb + a.PicThumb,
                                       PicFull = UrlNewsImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<NewsDto>();
            }
        }
        public async Task<List<GalleryDto>> GetGalleryList(string lang, int limit)
        {
            try
            {
                if (limit == 0)
                {
                    limit = 100;
                }

                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.GalleryDetails
                                   join b in _dbContext.MultiLang_GalleryDetails on a.Pid equals b.GalleryDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new GalleryDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       Slug = b.Slug,
                                       PicThumb = UrlGalleryImages + Thumb + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<GalleryDto>();
            }
        }
        public async Task<List<NewsDto>> GetHotList(string lang, int limit)
        {
            try
            {
                if (limit == 0)
                {
                    limit = 100;
                }

                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.NewsDetails
                                   join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.IsHot descending, a.Order descending
                                   select new NewsDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlNewsImages + Thumb + a.PicThumb,
                                       PicFull = UrlNewsImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();
                if (model.Any())
                {
                    model[0].PicThumb = model[0].PicFull;
                }
                return model;
            }
            catch (Exception ex)
            {
                return new List<NewsDto>();
            }
        }
        public async Task<List<NewsDto>> GetListBySlug(string lang, int page, string cate)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                var category = await (from a in _dbContext.NewsCates
                                      join b in _dbContext.MultiLang_NewsCates on a.Pid equals b.NewsCatePid
                                      where (!a.Deleted && a.Enabled && !a.isLocked) && (b.Slug == cate && b.LangKey == lang)
                                      select a).FirstOrDefaultAsync();

                var cateList = await (from a in _dbContext.NewsCates
                                      join b in _dbContext.MultiLang_NewsCates on a.Pid equals b.NewsCatePid
                                      where (!a.Deleted && a.Enabled && !a.isLocked && b.LangKey == lang)
                                      select a).ToListAsync();

                var result = new List<long>();
                result.Add(category.Pid);
                foreach (var item in cateList)
                {
                    var r = new List<long>();
                    var l = item.ParentRoute.Split('_');
                    if (l.Length > 0)
                    {
                        r = l.Where(x => !string.IsNullOrEmpty(x)).Select(x => Convert.ToInt64(x)).ToList();
                    }

                    if (r.Contains(category.Pid))
                    {
                        result.Add(item.Pid);
                    }
                }
                result = result.Distinct().ToList();

                var model = await (from a in _dbContext.NewsDetails
                                   join b in _dbContext.NewsCate_NewsDetails on a.Pid equals b.NewsDetailPid
                                   join c in _dbContext.MultiLang_NewsDetails on a.Pid equals c.NewsDetailPid
                                   where (!a.Deleted && a.Enabled && c.LangKey == lang && a.PublishDate <= DateTime.Now && result.Contains(b.NewsCatePid))
                                   orderby a.Order descending
                                   select new NewsDto
                                   {
                                       Pid = a.Pid,
                                       Title = c.Title,
                                       TitleAlt = c.Title.Replace("\"", "").Replace("'", ""),
                                       Content = c.Content,
                                       Description = c.Description,
                                       Slug = c.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlNewsImages + Thumb + a.PicThumb
                                   }).ToListAsync();
                return model.DistinctBy(x => x.Pid).ToList();
            }
            catch (Exception ex)
            {
                return new List<NewsDto>();
            }
        }
        public async Task<NewsDto> GetNews(string slug, string lang)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.NewsDetails
                                   join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                                   join c in _dbContext.NewsCate_NewsDetails on a.Pid equals c.NewsDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.Slug == slug && b.LangKey == lang)
                                   select new NewsDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       PublishDate = a.PublishDate.ToString("dddd, " + _common.ConvertFormatToCultureDateTime(DateFormat), vi),
                                       Slug = b.Slug,
                                       TagKey = a.TagKey,
                                       Enabled = a.Enabled,
                                       PicThumb = UrlNewsImages + Thumb + a.PicThumb,
                                       PicFull = UrlNewsImages + Fullmages + a.PicThumb,
                                       TitleSEO = b.TitleSEO,
                                       DescriptionSEO = b.DescriptionSEO,
                                       CateName = _dbContext.MultiLang_NewsCates
                                                   .Where(x => x.NewsCatePid == c.NewsCatePid && x.LangKey == lang)
                                                   .FirstOrDefault().Name,
                                       CateSlug = _dbContext.MultiLang_NewsCates
                                                   .Where(x => x.NewsCatePid == c.NewsCatePid && x.LangKey == lang)
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

                var common = await _dbContext.NewsDetails.Where(p => p.Pid == model.Pid && p.Enabled == true && p.Deleted == false).FirstOrDefaultAsync();
                common.CounterView = common.CounterView + 1;
                _dbContext.SaveChanges();

                model.ImageMeta = model.PicFull;
                model.TagKey = common.TagKey;
                model.SlugTagKey = common.SlugTagKey != null ? common.SlugTagKey : "";
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<NewsDto>> GetRelateList(string slug, string lang, int limit)
        {
            try
            {
                var categoryPids = await (from a in _dbContext.NewsDetails
                                          join b in _dbContext.NewsCate_NewsDetails on a.Pid equals b.NewsDetailPid
                                          join c in _dbContext.MultiLang_NewsDetails on a.Pid equals c.NewsDetailPid
                                          where (!a.Deleted && a.Enabled) && (c.Slug == slug) && (c.LangKey == lang)
                                          select b.NewsCatePid).ToListAsync();

                var model = await (from a in _dbContext.NewsDetails
                                   join b in _dbContext.MultiLang_NewsDetails on a.Pid equals b.NewsDetailPid
                                   join c in _dbContext.NewsCate_NewsDetails on a.Pid equals c.NewsDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang && b.Slug != slug) && (categoryPids.Contains(c.NewsCatePid))
                                   orderby a.Order descending
                                   select new NewsDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       //Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       //PublishDate = a.PublishDate.ToShortDateString(),
                                       PicThumb = UrlNewsImages + Thumb + a.PicThumb,
                                       PicFull = UrlNewsImages + Fullmages + a.PicThumb
                                   }).ToListAsync();
                model = model.DistinctBy(x => x.Pid).Take(limit).ToList();
                return model;
            }
            catch (Exception ex)
            {
                return new List<NewsDto>();
            }
        }
        public async Task<NewsCateDto> GetCate(string slug, string lang)
        {
            try
            {
                var model = await (from a in _dbContext.NewsCates
                                   join b in _dbContext.MultiLang_NewsCates on a.Pid equals b.NewsCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang && b.Slug == slug)
                                   orderby a.Order descending
                                   select new NewsCateDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Name,
                                       TitleAlt = b.Name.Replace("\"", "").Replace("'", ""),
                                       Slug = b.Slug,
                                       Description = b.Description,
                                   }).FirstOrDefaultAsync();
                if (model != null)
                {
                    return model;
                }
                return new NewsCateDto();
            }
            catch (Exception ex)
            {
                return new NewsCateDto();
            }
        }
        public async Task<List<NewsCateDto>> GetCateList(string lang)
        {
            try
            {
                var model = await (from a in _dbContext.NewsCates
                                   join b in _dbContext.MultiLang_NewsCates on a.Pid equals b.NewsCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new NewsCateDto
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
                return new List<NewsCateDto>();
            }
        }
        public NewsDto GetNewsPreview()
        {
            try
            {
                var news = new NewsDto();
                string lang = DefaultLang;
                CultureInfo vi = new CultureInfo(lang);
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == NewsId.ToString()).FirstOrDefault();
                var common = JsonConvert.DeserializeObject<NewsDetail>(model.Obj);
                var detail = JsonConvert.DeserializeObject<List<MultiLang_NewsDetail>>(model.ObjDetail).Where(x => x.LangKey == lang).FirstOrDefault();


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
                    var currentNews = _dbContext.NewsDetails.Where(x => x.Pid == common.Pid).FirstOrDefault();
                    if (currentNews != null)
                    {
                        news.PicThumb = UrlNewsImages + Thumb + currentNews.PicThumb;
                        news.PicFull = UrlNewsImages + Fullmages + currentNews.PicThumb;
                        news.ImageMeta = news.PicFull;
                    }
                }

                news.PublishDate = common.PublishDate.ToString("dddd, " + _common.ConvertFormatToCultureDateTime(DateFormat) + ",  hh:mm", vi);
                news.Slug = detail.Slug;
                news.TagKey = common.TagKey;
                return news;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
