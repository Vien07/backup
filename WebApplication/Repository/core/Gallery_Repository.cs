using CMS.Areas.Gallery.Models;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Gallery;
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
    public class Gallery_Repository : IGallery_Repository
    {
        private string DateFormat = "";
        private string PageLimit = "";

        private readonly ICommonServices _common;

        private readonly DBContext _dbContext;

        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string KeyDateFormat = ConstantStrings.KeyDateFormat;
        private string UrlGalleryImages = ConstantStrings.UrlGalleryImages;
        private string Thumb = ConstantStrings.Thumb;
        private string Fullmages = ConstantStrings.Fullmages;
        private string formatBase64 = ConstantStrings.formatBase64;
        private int GalleryId = ConstantStrings.GalleryId;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;

        public Gallery_Repository(DBContext dbContext, ICommonServices common)
        {
            _dbContext = dbContext;
            _common = common;
            DateFormat = _common.GetConfigValue(KeyDateFormat);
            PageLimit = _common.GetConfigValue(KeyPageLimit);
        }
        public async Task<List<GalleryDto>> GetList(string lang, int page)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.GalleryDetails
                                   join b in _dbContext.MultiLang_GalleryDetails on a.Pid equals b.GalleryDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new GalleryDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       //Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       //PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlGalleryImages + Thumb + a.PicThumb,
                                       VideoLink = a.VideoLink,
                                   }).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<GalleryDto>();
            }
        }
        public async Task<Dictionary<GalleryCateDto, List<GalleryDto>>> GetListDict(string lang, int limitCate, int limitList)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                Dictionary<GalleryCateDto, List<GalleryDto>> data = new Dictionary<GalleryCateDto, List<GalleryDto>>();

                var categories = await (from a in _dbContext.GalleryCates
                                        join b in _dbContext.MultiLang_GalleryCates on a.Pid equals b.GalleryCatePid
                                        where (!a.Deleted && a.Enabled && !a.isLocked) && b.LangKey == lang
                                        orderby a.Order descending
                                        select new GalleryCateDto { Pid = a.Pid, Title = b.Name, TitleAlt = b.Name.Replace("\"", "").Replace("'", ""), Slug = b.Slug }).Take(limitCate).ToListAsync();

                foreach (var item in categories)
                {

                    var model = await (from a in _dbContext.GalleryDetails
                                       join b in _dbContext.MultiLang_GalleryDetails on a.Pid equals b.GalleryDetailPid
                                       join c in _dbContext.GalleryCate_GalleryDetails on a.Pid equals c.GalleryDetailPid
                                       where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang) && c.GalleryCatePid == item.Pid
                                       orderby a.Order descending
                                       select new GalleryDto
                                       {
                                           Pid = a.Pid,
                                           Title = b.Title,
                                           TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                           Content = b.Content,
                                           Description = b.Description,
                                           Slug = b.Slug,
                                           PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                           PicThumb = UrlGalleryImages + Thumb + a.PicThumb
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
                return new Dictionary<GalleryCateDto, List<GalleryDto>>();
            }
        }
        public async Task<List<GalleryDto>> GetHighViewList(string lang, int limit)
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
                                   orderby a.CounterView descending
                                   select new GalleryDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlGalleryImages + Thumb + a.PicThumb,
                                       PicFull = UrlGalleryImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<GalleryDto>();
            }
        }
        public async Task<List<GalleryDto>> GetHotList(string lang, int limit)
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
                                   orderby a.IsHot descending, a.Order descending
                                   select new GalleryDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlGalleryImages + Thumb + a.PicThumb,
                                       PicFull = UrlGalleryImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<GalleryDto>();
            }
        }
        public async Task<List<GalleryDto>> GetListBySlug(string lang, int page, string cate)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                var category = await (from a in _dbContext.GalleryCates
                                      join b in _dbContext.MultiLang_GalleryCates on a.Pid equals b.GalleryCatePid
                                      where (!a.Deleted && a.Enabled && !a.isLocked) && (b.Slug == cate && b.LangKey == lang)
                                      select a).FirstOrDefaultAsync();

                var cateList = await (from a in _dbContext.GalleryCates
                                      join b in _dbContext.MultiLang_GalleryCates on a.Pid equals b.GalleryCatePid
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

                var model = await (from a in _dbContext.GalleryDetails
                                   join b in _dbContext.GalleryCate_GalleryDetails on a.Pid equals b.GalleryDetailPid
                                   join c in _dbContext.MultiLang_GalleryDetails on a.Pid equals c.GalleryDetailPid
                                   where (!a.Deleted && a.Enabled && c.LangKey == lang && a.PublishDate <= DateTime.Now && result.Contains(b.GalleryCatePid))
                                   orderby a.Order descending
                                   select new GalleryDto
                                   {
                                       Pid = a.Pid,
                                       Title = c.Title,
                                       TitleAlt = c.Title.Replace("\"", "").Replace("'", ""),
                                       Content = c.Content,
                                       Description = c.Description,
                                       Slug = c.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlGalleryImages + Thumb + a.PicThumb,
                                       VideoLink = a.VideoLink,
                                   }).ToListAsync();
                return model.DistinctBy(x => x.Pid).ToList();
            }
            catch (Exception ex)
            {
                return new List<GalleryDto>();
            }
        }
        public async Task<GalleryDto> GetGallery(string slug, string lang)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.GalleryDetails
                                   join b in _dbContext.MultiLang_GalleryDetails on a.Pid equals b.GalleryDetailPid
                                   join c in _dbContext.GalleryCate_GalleryDetails on a.Pid equals c.GalleryDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.Slug == slug && b.LangKey == lang)
                                   select new GalleryDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       PublishDate = a.PublishDate.ToString("dddd, " + _common.ConvertFormatToCultureDateTime(DateFormat) + ",  hh:mm", vi),
                                       Slug = b.Slug,
                                       TagKey = a.TagKey,
                                       Enabled = a.Enabled,
                                       PicThumb = UrlGalleryImages + Thumb + a.PicThumb,
                                       PicFull = UrlGalleryImages + Fullmages + a.PicThumb,
                                       TitleSEO = b.TitleSEO,
                                       DescriptionSEO = b.DescriptionSEO,
                                       VideoLink = a.VideoLink,
                                       CateName = _dbContext.MultiLang_GalleryCates
                                                   .Where(x => x.GalleryCatePid == c.GalleryCatePid && x.LangKey == lang)
                                                   .FirstOrDefault().Name,
                                       CateSlug = _dbContext.MultiLang_GalleryCates
                                                   .Where(x => x.GalleryCatePid == c.GalleryCatePid && x.LangKey == lang)
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

                var common = await _dbContext.GalleryDetails.Where(p => p.Pid == model.Pid && p.Enabled == true && p.Deleted == false).FirstOrDefaultAsync();
                common.CounterView = common.CounterView + 1;
                _dbContext.SaveChanges();

                #region image
                model.ImageMeta = model.PicFull;
                var imgs = new Dictionary<string, string>();
                var imgList = _dbContext.Images_Galleries.Where(x => x.GalleryDetailPid == model.Pid).ToList();

                imgs.Add(model.PicFull, model.Title);
                foreach (var item in imgList)
                {
                    var multiLangImage = _dbContext.MultiLang_Images_Galleries.Where(x => x.ImagesGalleryPid == item.Pid && x.LangKey == lang).FirstOrDefault();
                    if (multiLangImage == null)
                    {
                        imgs.Add(UrlGalleryImages + Fullmages + item.Images, "");
                    }
                    else
                    {
                        imgs.Add(UrlGalleryImages + Fullmages + item.Images, multiLangImage.Caption);
                    }
                }

                model.ImageList = imgs;
                #endregion
                model.TagKey = common.TagKey;
                model.SlugTagKey = common.SlugTagKey != null ? common.SlugTagKey : "";
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<GalleryDto>> GetRelateList(string slug, string lang, int limit)
        {
            try
            {
                var categoryPids = await (from a in _dbContext.GalleryDetails
                                          join b in _dbContext.GalleryCate_GalleryDetails on a.Pid equals b.GalleryDetailPid
                                          join c in _dbContext.MultiLang_GalleryDetails on a.Pid equals c.GalleryDetailPid
                                          where (!a.Deleted && a.Enabled) && (c.Slug == slug) && (c.LangKey == lang)
                                          select b.GalleryCatePid).ToListAsync();

                var model = await (from a in _dbContext.GalleryDetails
                                   join b in _dbContext.MultiLang_GalleryDetails on a.Pid equals b.GalleryDetailPid
                                   join c in _dbContext.GalleryCate_GalleryDetails on a.Pid equals c.GalleryDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang && b.Slug != slug) && (categoryPids.Contains(c.GalleryCatePid))
                                   orderby a.Order descending
                                   select new GalleryDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToShortDateString(),
                                       PicThumb = UrlGalleryImages + Thumb + a.PicThumb,
                                       PicFull = UrlGalleryImages + Fullmages + a.PicThumb,
                                       VideoLink = a.VideoLink,
                                   }).ToListAsync();
                model = model.DistinctBy(x => x.Pid).Take(limit).ToList();
                return model;
            }
            catch (Exception ex)
            {
                return new List<GalleryDto>();
            }
        }
        public async Task<GalleryCateDto> GetCate(string slug, string lang)
        {
            try
            {
                var model = await (from a in _dbContext.GalleryCates
                                   join b in _dbContext.MultiLang_GalleryCates on a.Pid equals b.GalleryCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang && b.Slug == slug)
                                   orderby a.Order descending
                                   select new GalleryCateDto
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
                return new GalleryCateDto();
            }
            catch (Exception ex)
            {
                return new GalleryCateDto();
            }
        }
        public async Task<List<GalleryCateDto>> GetCateList(string lang)
        {
            try
            {
                var model = await (from a in _dbContext.GalleryCates
                                   join b in _dbContext.MultiLang_GalleryCates on a.Pid equals b.GalleryCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new GalleryCateDto
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
                return new List<GalleryCateDto>();
            }
        }
        public GalleryDto GetGalleryPreview()
        {
            try
            {
                var gallery = new GalleryDto();
                string lang = DefaultLang;
                CultureInfo vi = new CultureInfo(lang);
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == GalleryId.ToString()).FirstOrDefault();
                var common = JsonConvert.DeserializeObject<GalleryDetail>(model.Obj);
                var detail = JsonConvert.DeserializeObject<List<MultiLang_GalleryDetail>>(model.ObjDetail).Where(x => x.LangKey == lang).FirstOrDefault();


                gallery.Title = detail.Title;
                gallery.TitleAlt = detail.Title.Replace("\"", "").Replace("'", "");
                gallery.Enabled = common.Enabled;
                gallery.Content = detail.Content;
                gallery.Description = detail.Description;
                gallery.Pid = common.Pid;

                if (model.IsEditPicThumb)
                {
                    gallery.PicThumb = UrlPreviewImages + Thumb + model.PicThumb;
                    gallery.PicFull = UrlPreviewImages + Fullmages + model.PicThumb;
                    gallery.ImageMeta = gallery.PicFull;
                }
                else
                {
                    var currentGallery = _dbContext.GalleryDetails.Where(x => x.Pid == common.Pid).FirstOrDefault();
                    if (currentGallery != null)
                    {
                        gallery.PicThumb = UrlGalleryImages + Thumb + currentGallery.PicThumb;
                        gallery.PicFull = UrlGalleryImages + Fullmages + currentGallery.PicThumb;
                        gallery.ImageMeta = gallery.PicFull;
                    }
                }

                gallery.PublishDate = common.PublishDate.ToString("dddd, " + _common.ConvertFormatToCultureDateTime(DateFormat) + ",  hh:mm", vi);
                gallery.Slug = detail.Slug;
                gallery.TagKey = common.TagKey;
                return gallery;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
