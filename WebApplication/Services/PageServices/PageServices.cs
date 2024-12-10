using CMS.Services.TranslateServices;
using DTO.Website;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using System.IO;
using MimeKit;
using CMS.Services.CommonServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CMS.Services.PageServices
{
    public class PageServices : IPageServices
    {
        private readonly ITranslateServices _translate;
        private readonly DBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonServices _common;
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private string DefaultOgImage = "";
        private string KeyDefaultOgImage = ConstantStrings.KeyDefaultOgImage;
        private string UrlBannerImages = ConstantStrings.UrlBannerImages;
        private string Fullmages = ConstantStrings.Fullmages;
        private string UrlBannerTemplate = ConstantStrings.UrlBannerTemplate;
        private string KeyMetaDescription = ConstantStrings.KeyMetaDescription;
        private string KeyWebsiteName = ConstantStrings.KeyWebsiteName;
        private string KeyRootDomain = ConstantStrings.KeyRootDomain;
        private string KeyMetaKeywords = ConstantStrings.KeyMetaKeywords;
        private string UrlMetaTemplate = ConstantStrings.UrlMetaTemplate;
        private string UrlConfigurationImages = ConstantStrings.UrlConfigurationImages;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string UrlSlideImages = ConstantStrings.UrlSlideImages;

        public PageServices(ITranslateServices translate, DBContext dbContext, IHttpContextAccessor httpContextAccessor, ICommonServices common, IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider)
        {
            _translate = translate;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _common = common;
            DefaultOgImage = _common.GetConfigValue(KeyDefaultOgImage);
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
        }

        public async Task<string> GetBanner(int pid)
        {
            string bannerImg = string.Empty;
            try
            {
                var img = await (from a in _dbContext.Banners
                                 join b in _dbContext.Banner_Pages on a.Pid equals b.BannerPid
                                 where b.PageId == pid && a.Enabled == true
                                                         && a.Deleted == false
                                 select new { Images = UrlBannerImages + Fullmages + a.Images }).FirstOrDefaultAsync();
                if (img == null)
                {
                    var data = _dbContext.Banners.ToList();
                    Random random = new Random();
                    int index = random.Next(data.Count);
                    bannerImg = UrlBannerImages + Fullmages + data[index].Images;
                    return bannerImg;
                }
                return img.Images;
            }
            catch (Exception ex)
            {
                return bannerImg;
            }
        }
        public string GetBanner(BannerDto data)
        {
            if (String.IsNullOrEmpty(data.Home))
            {
                data.Home = _translate.GetString("menu.home");
            }
            if (String.IsNullOrEmpty(data.HomeUrl))
            {
                data.HomeUrl = _translate.GetUrl("url.home");
            }


            var absolutepath = Directory.GetCurrentDirectory();
            var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlBannerTemplate, "banner.html");
            if (File.Exists(template))
            {
                StreamReader sr = new StreamReader(template);
                string content = sr.ReadToEnd(); sr.Close();
                content = content.Replace("{{urlHome}}", data.HomeUrl);
                content = content.Replace("{{home}}", data.Home);
                content = content.Replace("{{banner}}", data.Banner);
                content = content.Replace("{{currentPage}}", data.CurrentPage);

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = content;
                return bodyBuilder.HtmlBody;
            }
            return "";
        }
        public string GetMeta(MetaDto data)
        {
            string _websiteName = _common.GetConfigValue(KeyWebsiteName);
            string _rootDomain = _common.GetConfigValue(KeyRootDomain);
            string _des = _common.GetConfigValue(KeyMetaDescription);
            string _keyw = _common.GetConfigValue(KeyMetaKeywords);
            if (data.Description == "" || data.Description == null)
            {
                data.Description = _des;
            }
            if (data.Keywords == "" || data.Keywords == null)
            {
                data.Keywords = _keyw;

            }
            data.SiteName = _websiteName; // WebsiteName;
            data.SiteTitle = _websiteName; // WebsiteName;
            string _host = _rootDomain;
            data.HomepageUrl = _host;
            var absolutepath = Directory.GetCurrentDirectory();
            var metaTag = "";
            var keywords = "";
            if (data.Keywords != null && data.Keywords != "")
            {
                keywords = "\n<meta name=\"keywords\" content=\"" + data.Keywords.Trim() + "\" />";
            }
            if (data.Keywords != null && data.Keywords != "")
            {
                var tagkeyArr = data.Keywords.Split(',');


                foreach (var item in tagkeyArr)
                {
                    metaTag += "\n<meta property=\"article:tag\" content=\"" + item.Trim() + "\"/>";
                }
            }
            var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlMetaTemplate, "meta.html");
            if (File.Exists(template))
            {
                StreamReader sr = new StreamReader(template);
                string content = sr.ReadToEnd(); sr.Close();
                content = content.Replace("{{pageTitle}}", data.PageTitle);
                content = content.Replace("{{description}}", data.Description);
                //content = content.Replace("{{keywords}}", data.Keywords);
                if (String.IsNullOrEmpty(data.PageUrl))
                {
                    content = content.Replace("{{pageUrl}}", _host);

                }
                else
                {
                    content = content.Replace("{{pageUrl}}", _host + data.PageUrl);

                }
                if (String.IsNullOrEmpty(data.ImageUrl))
                {
                    content = content.Replace("{{imageUrl}}", _host + UrlConfigurationImages + DefaultOgImage);
                }
                else
                {
                    content = content.Replace("{{imageUrl}}", _host + data.ImageUrl);
                }
                content = content.Replace("{{homepageUrl}}", data.HomepageUrl);
                content = content.Replace("{{siteName}}", data.SiteName);
                content = content.Replace("{{siteTitle}}", data.SiteTitle);
                content = content.Replace("{{meta-tag}}", metaTag);
                content = content.Replace("{{meta-keywords}}", keywords);
                content = content.Replace("{{ogType}}", data.OgType);
                content += "\t<meta name=\"author\" content=\"BizMaC\" />";
                content += "\n\t<meta name=\"generator\" content=\"BizMaC(https://www.bizmac.com)\" />";
                content += "\n\t<meta name=\"copyright\" content=\"BizMaC, Thiết kế website chuyên nghiệp\" />";

                if (!data.Is404Page)
                {
                    content += "\n\t<meta name=\"search\" content=\"always\" />";
                    content += "\n\t<meta name=\"distribution\" content=\"BizMaC, Thiết kế website chuyên nghiệp\" />";
                    content += "\n\t<meta name=\"revisit-after\" content=\"1 day\" />";
                    content += "\n\t<meta name=\"robots\" content=\"index,follow\" />";
                }
                else
                {
                    content += "\n\t<meta name=\"search\" content=\"always\" />";
                    content += "\n\t<meta name=\"distribution\" content=\"BizMaC, Thiết kế website chuyên nghiệp\" />";
                    content += "\n\t<meta name=\"revisit-after\" content=\"1 day\" />";
                    content += "\n\t<meta name=\"robots\" content=\"noindex,nofollow\" />";
                    content += "\n\t<meta name=\"googlebot\" content=\"noindex\" />";
                }


                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = content;
                return bodyBuilder.HtmlBody;
            }
            return "";
        }
        public async Task<List<SlideDto>> GetSlide()
        {
            try
            {
                string lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                var model = (from a in _dbContext.Slides
                             join b in _dbContext.MultiLang_Slides on a.Pid equals b.SlidePid
                             where (!a.Deleted && a.Enabled) && (b.LangKey == lang)
                             orderby a.Order descending
                             select new SlideDto
                             {
                                 Position = a.Position,
                                 Link = a.Link,
                                 Title = b.Title,
                                 Description = b.Description,
                                 TitleAlt = b.Title.Replace("\"", "").Replace("'", ""),
                                 Images = UrlSlideImages + Fullmages + a.Images,
                                 TargetLink = a.TargetLink
                             }).ToListAsync();

                return await model;
            }
            catch (Exception ex)
            {
                return new List<SlideDto>();
            }
        }
        public async Task<List<AdvertisementDto>> GetAdvertisements(long modulePid)
        {
            try
            {
                string lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                var list = (from a in _dbContext.Advertisements
                            join b in _dbContext.Advertisement_Pages on a.Pid equals b.AdvertisementPid
                            join c in _dbContext.MultiLang_Advertisements on a.Pid equals c.AdvertisementPid
                            where (!a.Deleted && a.Enabled) && (b.PageId == modulePid && c.LangKey == lang)
                            orderby a.Order descending
                            select new AdvertisementDto
                            {
                                Link = a.Link,
                                Title = c.Title,
                                TitleAlt = c.Title.Replace("\"", "").Replace("'", ""),
                                Image = ConstantStrings.UrlAdvertisementImages + Fullmages + a.Images,
                                DisplayType = a.DisplayType,
                                EmbedCode = c.EmbedCode,
                                 TargetLink = a.TargetLink
                            }).Take(5).ToListAsync();
                return await list;
            }
            catch
            {
                return new List<AdvertisementDto>();
            }
        }
        public async Task<PopupDto> GetPopup(long modulePid)
        {
            try
            {
                string lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                var model = (from a in _dbContext.Popups
                             join b in _dbContext.Popup_Pages on a.Pid equals b.PopupPid
                             join c in _dbContext.MultiLang_Popups on a.Pid equals c.PopupPid
                             where (!a.Deleted && a.Enabled) && (b.PageId == modulePid && c.LangKey == lang)
                             orderby a.Order descending
                             select new PopupDto
                             {
                                 Link = a.Link,
                                 Title = c.Title,
                                 TitleAlt = c.Title.Replace("\"", "").Replace("'", ""),
                                 Image = ConstantStrings.UrlPopupImages + Fullmages + a.Images,
                                 DisplayType = a.DisplayType,
                                 EmbedCode = c.EmbedCode,
                                 DelayTime = a.DelayTime,
                                 TargetLink = a.TargetLink
                             }).FirstOrDefaultAsync();
                return await model;
            }
            catch
            {
                return null;
            }
        }
        public async Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model)
        {
            var actionContext = GetActionContext();
            var view = FindView(actionContext, viewName);
            var viewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    viewData,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    output,
                    new HtmlHelperOptions()
                );
                await view.RenderAsync(viewContext);
                return output.ToString();
            }
        }
        private IView FindView(ActionContext actionContext, string viewName)
        {
            var getViewResult = _viewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult.View;
            }

            var findViewResult = _viewEngine.FindView(actionContext, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult.View;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations)); ;

            throw new InvalidOperationException(errorMessage);
        }
        private ActionContext GetActionContext()
        {
            return new ActionContext(_httpContextAccessor.HttpContext, _httpContextAccessor.HttpContext.GetRouteData(), new ActionDescriptor());
        }
    }
}
