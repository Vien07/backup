using CMS.Areas.Feature.Models;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.Feature;
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
    public class Feature_Repository : IFeature_Repository
    {
        private string DateFormat = "";
        private string PageLimit = "";

        private readonly ICommonServices _common;

        private readonly DBContext _dbContext;

        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string KeyDateFormat = ConstantStrings.KeyDateFormat;
        private string UrlFeatureImages = ConstantStrings.UrlFeatureImages;
        private string Thumb = ConstantStrings.Thumb;
        private string Fullmages = ConstantStrings.Fullmages;
        private string formatBase64 = ConstantStrings.formatBase64;
        private int FeatureId = ConstantStrings.FeatureId;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;

        public Feature_Repository(DBContext dbContext, ICommonServices common)
        {
            _dbContext = dbContext;
            _common = common;
            DateFormat = _common.GetConfigValue(KeyDateFormat);
            PageLimit = _common.GetConfigValue(KeyPageLimit);
        }
        public async Task<List<FeatureDto>> GetList(string lang, int page)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.FeatureDetails
                                   join b in _dbContext.MultiLang_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new FeatureDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       //Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       //PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlFeatureImages + Thumb + a.PicThumb,
                                       PicFull = UrlFeatureImages + Fullmages + a.PicThumb
                                   }).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<FeatureDto>();
            }
        }
        public async Task<Dictionary<FeatureCateDto, List<FeatureDto>>> GetListDict(string lang, int limitCate, int limitList)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                Dictionary<FeatureCateDto, List<FeatureDto>> data = new Dictionary<FeatureCateDto, List<FeatureDto>>();

                var categories = await (from a in _dbContext.FeatureCates
                                        join b in _dbContext.MultiLang_FeatureCates on a.Pid equals b.FeatureCatePid
                                        where (!a.Deleted && a.Enabled && !a.isLocked) && b.LangKey == lang
                                        orderby a.Order descending
                                        select new FeatureCateDto { Pid = a.Pid, Title = b.Name, TitleAlt = b.Name.Replace("\"", "").Replace("'", ""), Slug = b.Slug }).Take(limitCate).ToListAsync();

                foreach (var item in categories)
                {

                    var model = await (from a in _dbContext.FeatureDetails
                                       join b in _dbContext.MultiLang_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                       join c in _dbContext.FeatureCate_FeatureDetails on a.Pid equals c.FeatureDetailPid
                                       where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang) && c.FeatureCatePid == item.Pid
                                       orderby a.Order descending
                                       select new FeatureDto
                                       {
                                           Pid = a.Pid,
                                           Title = b.Title,
                                           TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                           Content = b.Content,
                                           Description = b.Description,
                                           Slug = b.Slug,
                                           PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                           PicThumb = UrlFeatureImages + Thumb + a.PicThumb
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
                return new Dictionary<FeatureCateDto, List<FeatureDto>>();
            }
        }
        public async Task<List<FeatureDto>> GetHighViewList(string lang, int limit)
        {
            try
            {
                if (limit == 0)
                {
                    limit = 100;
                }

                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.FeatureDetails
                                   join b in _dbContext.MultiLang_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.CounterView descending
                                   select new FeatureDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlFeatureImages + Thumb + a.PicThumb,
                                       PicFull = UrlFeatureImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<FeatureDto>();
            }
        }
        public async Task<List<FeatureDto>> GetHotList(string lang, int limit)
        {
            try
            {
                if (limit == 0)
                {
                    limit = 100;
                }

                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.FeatureDetails
                                   join b in _dbContext.MultiLang_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.IsHot descending, a.Order descending
                                   select new FeatureDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       //Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       //PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlFeatureImages + Thumb + a.PicThumb,
                                       PicFull = UrlFeatureImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<FeatureDto>();
            }
        }
        public async Task<List<FeatureDto>> GetListBySlug(string lang, int page, string cate)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                var category = await (from a in _dbContext.FeatureCates
                                      join b in _dbContext.MultiLang_FeatureCates on a.Pid equals b.FeatureCatePid
                                      where (!a.Deleted && a.Enabled && !a.isLocked) && (b.Slug == cate && b.LangKey == lang)
                                      select a).FirstOrDefaultAsync();

                var cateList = await (from a in _dbContext.FeatureCates
                                      join b in _dbContext.MultiLang_FeatureCates on a.Pid equals b.FeatureCatePid
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

                var model = await (from a in _dbContext.FeatureDetails
                                   join b in _dbContext.FeatureCate_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                   join c in _dbContext.MultiLang_FeatureDetails on a.Pid equals c.FeatureDetailPid
                                   where (!a.Deleted && a.Enabled && c.LangKey == lang && a.PublishDate <= DateTime.Now && result.Contains(b.FeatureCatePid))
                                   orderby a.Order descending
                                   select new FeatureDto
                                   {
                                       Pid = a.Pid,
                                       TitleAlt = c.Title.Replace("\"", "").Replace("'", ""),
                                       Title = c.Title,
                                       Content = c.Content,
                                       Description = c.Description,
                                       Slug = c.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlFeatureImages + Thumb + a.PicThumb
                                   }).ToListAsync();
                return model.DistinctBy(x => x.Pid).ToList();
            }
            catch (Exception ex)
            {
                return new List<FeatureDto>();
            }
        }
        public async Task<FeatureDto> GetFeature(string slug, string lang)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.FeatureDetails
                                   join b in _dbContext.MultiLang_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.Slug == slug && b.LangKey == lang)
                                   select new FeatureDto
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
                                       PicThumb = UrlFeatureImages + Thumb + a.PicThumb,
                                       PicFull = UrlFeatureImages + Fullmages + a.PicThumb,
                                       TitleSEO = b.TitleSEO,
                                       DescriptionSEO = b.DescriptionSEO,
                                   }).FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(model.TitleSEO))
                {
                    model.TitleSEO = model.Title;
                }
                if (string.IsNullOrEmpty(model.DescriptionSEO))
                {
                    model.DescriptionSEO = model.Description;
                }

                var common = await _dbContext.FeatureDetails.Where(p => p.Pid == model.Pid && p.Enabled == true && p.Deleted == false).FirstOrDefaultAsync();
                common.CounterView = common.CounterView + 1;
                _dbContext.SaveChanges();

                #region image
                model.ImageMeta = model.PicFull;

                var imgs = new Dictionary<string, string>();
                var imgList = _dbContext.Images_Features.Where(x => x.FeatureDetailPid == model.Pid).ToList();
                imgs.Add(model.PicFull, model.Title);
                foreach (var item in imgList)
                {
                    var multiLangImage = _dbContext.MultiLang_Images_Features.Where(x => x.ImagesFeaturePid == item.Pid && x.LangKey == lang).FirstOrDefault();
                    if (multiLangImage == null)
                    {
                        imgs.Add(UrlFeatureImages + Fullmages + item.Images, "");
                    }
                    else
                    {
                        imgs.Add(UrlFeatureImages + Fullmages + item.Images, multiLangImage.Caption);
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
        public async Task<List<FeatureDto>> GetRelateList(string slug, string lang, int limit)
        {
            try
            {
                var categoryPids = await (from a in _dbContext.FeatureDetails
                                          join b in _dbContext.FeatureCate_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                          join c in _dbContext.MultiLang_FeatureDetails on a.Pid equals c.FeatureDetailPid
                                          where (!a.Deleted && a.Enabled) && (c.Slug == slug) && (c.LangKey == lang)
                                          select b.FeatureCatePid).ToListAsync();

                var model = await (from a in _dbContext.FeatureDetails
                                   join b in _dbContext.MultiLang_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                   join c in _dbContext.FeatureCate_FeatureDetails on a.Pid equals c.FeatureDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang && b.Slug != slug) && (categoryPids.Contains(c.FeatureCatePid))
                                   orderby a.Order descending
                                   select new FeatureDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToShortDateString(),
                                       PicThumb = UrlFeatureImages + Thumb + a.PicThumb,
                                       PicFull = UrlFeatureImages + Fullmages + a.PicThumb
                                   }).ToListAsync();
                model = model.DistinctBy(x => x.Pid).Take(limit).ToList();
                return model;
            }
            catch (Exception ex)
            {
                return new List<FeatureDto>();
            }
        }
        public async Task<FeatureCateDto> GetCate(string slug, string lang)
        {
            try
            {
                var model = await (from a in _dbContext.FeatureCates
                                   join b in _dbContext.MultiLang_FeatureCates on a.Pid equals b.FeatureCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang && b.Slug == slug)
                                   orderby a.Order descending
                                   select new FeatureCateDto
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
                return new FeatureCateDto();
            }
            catch (Exception ex)
            {
                return new FeatureCateDto();
            }
        }
        public async Task<List<FeatureCateDto>> GetCateList(string lang)
        {
            try
            {
                var model = await (from a in _dbContext.FeatureCates
                                   join b in _dbContext.MultiLang_FeatureCates on a.Pid equals b.FeatureCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new FeatureCateDto
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
                return new List<FeatureCateDto>();
            }
        }
        public FeatureDto GetFeaturePreview()
        {
            try
            {
                var feature = new FeatureDto();
                string lang = DefaultLang;
                CultureInfo vi = new CultureInfo(lang);
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == FeatureId.ToString()).FirstOrDefault();
                var common = JsonConvert.DeserializeObject<FeatureDetail>(model.Obj);
                var detail = JsonConvert.DeserializeObject<List<MultiLang_FeatureDetail>>(model.ObjDetail).Where(x => x.LangKey == lang).FirstOrDefault();


                feature.Title = detail.Title;
                feature.TitleAlt = detail.Title.Replace("\"", "").Replace("'", "");
                feature.Enabled = common.Enabled;
                feature.Content = detail.Content;
                feature.Description = detail.Description;
                feature.Pid = common.Pid;

                if (model.IsEditPicThumb)
                {
                    feature.PicThumb = UrlPreviewImages + Thumb + model.PicThumb;
                    feature.PicFull = UrlPreviewImages + Fullmages + model.PicThumb;
                    feature.ImageMeta = feature.PicFull;
                }
                else
                {
                    var currentFeature = _dbContext.FeatureDetails.Where(x => x.Pid == common.Pid).FirstOrDefault();
                    if (currentFeature != null)
                    {
                        feature.PicThumb = UrlFeatureImages + Thumb + currentFeature.PicThumb;
                        feature.PicFull = UrlFeatureImages + Fullmages + currentFeature.PicThumb;
                        feature.ImageMeta = feature.PicFull;
                    }
                }

                feature.PublishDate = common.PublishDate.ToString("dddd, " + _common.ConvertFormatToCultureDateTime(DateFormat) + ",  hh:mm", vi);
                feature.Slug = detail.Slug;
                feature.TagKey = common.TagKey;
                return feature;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<FeatureDto>> GetFeatureListForForm(string lang)
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
        public async Task<string> GetSlugDefault(string lang)
        {
            try
            {
                var slug = await (from a in _dbContext.FeatureDetails
                                  join b in _dbContext.MultiLang_FeatureDetails on a.Pid equals b.FeatureDetailPid
                                  orderby a.Order descending
                                  where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                  select b.Slug).FirstOrDefaultAsync();
                return slug;
            }
            catch
            {
                return "";
            }
        }
    }
}
