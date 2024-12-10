using CMS.Areas.FAQ.Models;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using DTO;
using DTO.FAQ;
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
    public class FAQ_Repository : IFAQ_Repository
    {
        private string DateFormat = "";
        private string PageLimit = "";

        private readonly ICommonServices _common;

        private readonly DBContext _dbContext;

        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string KeyDateFormat = ConstantStrings.KeyDateFormat;
        private string UrlFAQImages = ConstantStrings.UrlFAQImages;
        private string Thumb = ConstantStrings.Thumb;
        private string Fullmages = ConstantStrings.Fullmages;
        private string formatBase64 = ConstantStrings.formatBase64;
        private int FAQId = ConstantStrings.FAQId;
        private string UrlPreviewImages = ConstantStrings.UrlPreviewImages;

        public FAQ_Repository(DBContext dbContext, ICommonServices common)
        {
            _dbContext = dbContext;
            _common = common;
            DateFormat = _common.GetConfigValue(KeyDateFormat);
            PageLimit = _common.GetConfigValue(KeyPageLimit);
        }
        public async Task<List<FAQDto>> GetList(string lang, int page)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.FAQDetails
                                   join b in _dbContext.MultiLang_FAQDetails on a.Pid equals b.FAQDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new FAQDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Answer = b.Answer,
                                       //Content = b.Content,
                                       //Description = b.Description,
                                       //Slug = b.Slug,
                                       //PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       //PicThumb = UrlFAQImages + Thumb + a.PicThumb
                                   }).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<FAQDto>();
            }
        }
        public async Task<Dictionary<FAQCateDto, List<FAQDto>>> GetListDict(string lang, int limitCate, int limitList)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                Dictionary<FAQCateDto, List<FAQDto>> data = new Dictionary<FAQCateDto, List<FAQDto>>();

                var categories = await (from a in _dbContext.FAQCates
                                        join b in _dbContext.MultiLang_FAQCates on a.Pid equals b.FAQCatePid
                                        where (!a.Deleted && a.Enabled && !a.isLocked) && b.LangKey == lang
                                        orderby a.Order descending
                                        select new FAQCateDto { Pid = a.Pid, Title = b.Name, TitleAlt = b.Name.Replace("\"", "").Replace("'", ""), Slug = b.Slug }).Take(limitCate).ToListAsync();

                foreach (var item in categories)
                {

                    var model = await (from a in _dbContext.FAQDetails
                                       join b in _dbContext.MultiLang_FAQDetails on a.Pid equals b.FAQDetailPid
                                       join c in _dbContext.FAQCate_FAQDetails on a.Pid equals c.FAQDetailPid
                                       where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang) && c.FAQCatePid == item.Pid
                                       orderby a.Order descending
                                       select new FAQDto
                                       {
                                           Pid = a.Pid,
                                           Title = b.Title,
                                           TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                           Content = b.Content,
                                           Description = b.Description,
                                           Slug = b.Slug,
                                           PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                           PicThumb = UrlFAQImages + Thumb + a.PicThumb
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
                return new Dictionary<FAQCateDto, List<FAQDto>>();
            }
        }
        public async Task<List<FAQDto>> GetHighViewList(string lang, int limit)
        {
            try
            {
                if (limit == 0)
                {
                    limit = 100;
                }

                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.FAQDetails
                                   join b in _dbContext.MultiLang_FAQDetails on a.Pid equals b.FAQDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   orderby a.CounterView descending
                                   select new FAQDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlFAQImages + Thumb + a.PicThumb,
                                       PicFull = UrlFAQImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<FAQDto>();
            }
        }
        public async Task<List<FAQDto>> GetHotList(string lang, int limit)
        {
            try
            {
                if (limit == 0)
                {
                    limit = 100;
                }

                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.FAQDetails
                                   join b in _dbContext.MultiLang_FAQDetails on a.Pid equals b.FAQDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                   orderby a.IsHot descending, a.Order descending
                                   select new FAQDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlFAQImages + Thumb + a.PicThumb,
                                       PicFull = UrlFAQImages + Fullmages + a.PicThumb,
                                   }).Take(limit).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {
                return new List<FAQDto>();
            }
        }
        public async Task<List<FAQDto>> GetListBySlug(string lang, int page, string cate)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                var category = await (from a in _dbContext.FAQCates
                                      join b in _dbContext.MultiLang_FAQCates on a.Pid equals b.FAQCatePid
                                      where (!a.Deleted && a.Enabled && !a.isLocked) && (b.Slug == cate && b.LangKey == lang)
                                      select a).FirstOrDefaultAsync();

                var cateList = await (from a in _dbContext.FAQCates
                                      join b in _dbContext.MultiLang_FAQCates on a.Pid equals b.FAQCatePid
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

                var model = await (from a in _dbContext.FAQDetails
                                   join b in _dbContext.FAQCate_FAQDetails on a.Pid equals b.FAQDetailPid
                                   join c in _dbContext.MultiLang_FAQDetails on a.Pid equals c.FAQDetailPid
                                   where (!a.Deleted && a.Enabled && c.LangKey == lang && a.PublishDate <= DateTime.Now && result.Contains(b.FAQCatePid))
                                   orderby a.Order descending
                                   select new FAQDto
                                   {
                                       Pid = a.Pid,
                                       Title = c.Title,
                                       TitleAlt = c.Title.Replace("\"", "").Replace("'", ""),
                                       Content = c.Content,
                                       Description = c.Description,
                                       Slug = c.Slug,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm ", vi),
                                       PicThumb = UrlFAQImages + Thumb + a.PicThumb
                                   }).ToListAsync();
                return model.DistinctBy(x => x.Pid).ToList();
            }
            catch (Exception ex)
            {
                return new List<FAQDto>();
            }
        }
        public async Task<FAQDto> GetFAQ(string slug, string lang)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);

                var model = await (from a in _dbContext.FAQDetails
                                   join b in _dbContext.MultiLang_FAQDetails on a.Pid equals b.FAQDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.Slug == slug && b.LangKey == lang)
                                   select new FAQDto
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
                                       PicThumb = UrlFAQImages + Thumb + a.PicThumb,
                                       PicFull = UrlFAQImages + Fullmages + a.PicThumb,
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

                var common = await _dbContext.FAQDetails.Where(p => p.Pid == model.Pid && p.Enabled == true && p.Deleted == false).FirstOrDefaultAsync();
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
        public async Task<List<FAQDto>> GetRelateList(string slug, string lang, int limit)
        {
            try
            {
                var categoryPids = await (from a in _dbContext.FAQDetails
                                          join b in _dbContext.FAQCate_FAQDetails on a.Pid equals b.FAQDetailPid
                                          join c in _dbContext.MultiLang_FAQDetails on a.Pid equals c.FAQDetailPid
                                          where (!a.Deleted && a.Enabled) && (c.Slug == slug) && (c.LangKey == lang)
                                          select b.FAQCatePid).ToListAsync();

                var model = await (from a in _dbContext.FAQDetails
                                   join b in _dbContext.MultiLang_FAQDetails on a.Pid equals b.FAQDetailPid
                                   join c in _dbContext.FAQCate_FAQDetails on a.Pid equals c.FAQDetailPid
                                   where (!a.Deleted && a.Enabled && a.PublishDate <= DateTime.Now) && (b.LangKey == lang && b.Slug != slug) && (categoryPids.Contains(c.FAQCatePid))
                                   orderby a.Order descending
                                   select new FAQDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       Slug = b.Slug,
                                       PublishDate = a.PublishDate.ToShortDateString(),
                                       PicThumb = UrlFAQImages + Thumb + a.PicThumb,
                                       PicFull = UrlFAQImages + Fullmages + a.PicThumb
                                   }).ToListAsync();
                model = model.DistinctBy(x => x.Pid).Take(limit).ToList();
                return model;
            }
            catch (Exception ex)
            {
                return new List<FAQDto>();
            }
        }
        public async Task<FAQCateDto> GetCate(string slug, string lang)
        {
            try
            {
                var model = await (from a in _dbContext.FAQCates
                                   join b in _dbContext.MultiLang_FAQCates on a.Pid equals b.FAQCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang && b.Slug == slug)
                                   orderby a.Order descending
                                   select new FAQCateDto
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
                return new FAQCateDto();
            }
            catch (Exception ex)
            {
                return new FAQCateDto();
            }
        }
        public async Task<List<FAQCateDto>> GetCateList(string lang)
        {
            try
            {
                var model = await (from a in _dbContext.FAQCates
                                   join b in _dbContext.MultiLang_FAQCates on a.Pid equals b.FAQCatePid
                                   where (!a.Deleted && a.Enabled && !a.isLocked) && (b.LangKey == lang)
                                   orderby a.Order descending
                                   select new FAQCateDto
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
                return new List<FAQCateDto>();
            }
        }
        public FAQDto GetFAQPreview()
        {
            try
            {
                var faq = new FAQDto();
                string lang = DefaultLang;
                CultureInfo vi = new CultureInfo(lang);
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == FAQId.ToString()).FirstOrDefault();
                var common = JsonConvert.DeserializeObject<FAQDetail>(model.Obj);
                var detail = JsonConvert.DeserializeObject<List<MultiLang_FAQDetail>>(model.ObjDetail).Where(x => x.LangKey == lang).FirstOrDefault();


                faq.Title = detail.Title;
                faq.TitleAlt = detail.Title.Replace("\"", "").Replace("'", "");
                faq.Enabled = common.Enabled;
                faq.Content = detail.Content;
                faq.Description = detail.Description;
                faq.Pid = common.Pid;

                if (model.IsEditPicThumb)
                {
                    faq.PicThumb = UrlPreviewImages + Thumb + model.PicThumb;
                    faq.PicFull = UrlPreviewImages + Fullmages + model.PicThumb;
                    faq.ImageMeta = faq.PicFull;
                }
                else
                {
                    var currentFAQ = _dbContext.FAQDetails.Where(x => x.Pid == common.Pid).FirstOrDefault();
                    if (currentFAQ != null)
                    {
                        faq.PicThumb = UrlFAQImages + Thumb + currentFAQ.PicThumb;
                        faq.PicFull = UrlFAQImages + Fullmages + currentFAQ.PicThumb;
                        faq.ImageMeta = faq.PicFull;
                    }
                }

                faq.PublishDate = common.PublishDate.ToString("dddd, " + _common.ConvertFormatToCultureDateTime(DateFormat) + ",  hh:mm", vi);
                faq.Slug = detail.Slug;
                faq.TagKey = common.TagKey;
                return faq;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
