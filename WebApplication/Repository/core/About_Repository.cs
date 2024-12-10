using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using CMS.Areas.Contact.Models;
using X.PagedList;
using System.Globalization;
using CMS.Areas.About.Models;
using Newtonsoft.Json;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.About;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CMS.Repository
{
    public class About_Repository : IAbout_Repository
    {
        private string DefaultOgImage = "";
        private string PageLimitDetail = "";
        private string PageLimit = "";
        private readonly ICommonServices _core;

        private readonly DBContext _dbContext;

        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string UrlAboutImages = ConstantStrings.UrlAboutImages;
        private string Thumb = ConstantStrings.Thumb;
        private string UrlConfigurationImages = ConstantStrings.UrlConfigurationImages;
        private string KeyDefaultOgImage = ConstantStrings.KeyDefaultOgImage;
        private string KeyPageLimitDetail = ConstantStrings.KeyPageLimitDetail;
        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private int AboutId = ConstantStrings.AboutId;
        public About_Repository(DBContext dbContext, ICommonServices core)
        {
            _dbContext = dbContext;
            _core = core;
            DefaultOgImage = _core.GetConfigValue(KeyDefaultOgImage);
            PageLimitDetail = _core.GetConfigValue(KeyPageLimitDetail);
            PageLimit = _core.GetConfigValue(KeyPageLimit);
        }
        public async Task<AboutDto> GetAbout(string slug, string lang)
        {
            try
            {
                CultureInfo vi = new CultureInfo(lang);
                var model = await (from a in _dbContext.AboutDetails
                                   join b in _dbContext.MultiLang_AboutDetails on a.Pid equals b.AboutDetailPid
                                   where (!a.Deleted && a.Enabled) && (b.Slug == slug)
                                   select new AboutDto
                                   {
                                       Pid = a.Pid,
                                       Title = b.Title,
                                       TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                       Content = b.Content,
                                       Description = b.Description,
                                       OrgImages = UrlConfigurationImages + DefaultOgImage,
                                       PublishDate = a.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm '(GMT+7)'", vi),
                                       Slug = b.Slug,
                                       TagKey = a.TagKey,
                                       Enabled = a.Enabled,
                                       Default = a.Default,
                                   }).FirstOrDefaultAsync();

                var common = _dbContext.AboutDetails.Where(p => p.Pid == model.Pid && p.Enabled == true && p.Deleted == false).FirstOrDefault();
                common.CounterView = common.CounterView + 1;
                _dbContext.SaveChanges();

                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<string> GetSlugDefault(string lang)
        {
            try
            {
                var slug = await (from a in _dbContext.AboutDetails
                                  join b in _dbContext.MultiLang_AboutDetails on a.Pid equals b.AboutDetailPid
                                  orderby a.Default descending, a.Order descending
                                  where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                                  select b.Slug).FirstOrDefaultAsync();
                return slug;
            }
            catch
            {
                return "";
            }
        }
        public AboutDto GetAboutPreview()
        {
            try
            {
                AboutDto about = new AboutDto();
                string lang = DefaultLang;
                CultureInfo vi = new CultureInfo(lang);
                var model = _dbContext.ModulePreviews.Where(x => x.ModuleId == AboutId.ToString()).FirstOrDefault();
                var common = JsonConvert.DeserializeObject<AboutDetail>(model.Obj);
                var detail = JsonConvert.DeserializeObject<List<MultiLang_AboutDetail>>(model.ObjDetail).Where(x => x.LangKey == lang).FirstOrDefault();


                about.Title = detail.Title;
                about.TitleAlt = detail.Title.Replace("\"", "").Replace("'", "");
                about.Content = detail.Content;
                about.Description = detail.Description;
                about.OrgImages = UrlConfigurationImages + DefaultOgImage;
                about.PublishDate = common.PublishDate.ToString("dddd, dd/M/yyyy, hh:mm '(GMT+7)'", vi);
                about.Slug = detail.Slug;
                about.TagKey = common.TagKey;
                about.Enabled = common.Enabled;
                return about;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
