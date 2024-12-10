using System;
using Microsoft.AspNetCore.Mvc;
using CMS.Repository;
using CMS.Services.WebsiteServices;
using CMS.Services.PageServices;
using DTO.Website;
using DTO;
using CMS.Services.CommonServices;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebsiteServices _website;
        private readonly IHome_Repository _rep;
        private readonly ICommonServices _common;
        private readonly IPageServices _page;
        private readonly ICustomer_Repository _repCustomer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string KeyWebsiteName = ConstantStrings.KeyWebsiteName;
        private string OgTypeWebsite = ConstantStrings.OgTypeWebsite;
        private string DefaultLang = ConstantStrings.DefaultLang;
        public HomeController(IHome_Repository rep, IWebsiteServices website, ICommonServices common, IPageServices page, IHttpContextAccessor httpContextAccessor, ICustomer_Repository repCustomer)
        {
            _rep = rep;
            _repCustomer = repCustomer;
            _common = common;
            _website = website;
            _page = page;
            _httpContextAccessor = httpContextAccessor;
            _website.StartUp();
            _website.SetVisitor();
        }
        public async Task<IActionResult> Index(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                ViewBag.Slide = await _page.GetSlide();
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.HomeId);
                string WebsiteName = _common.GetConfigValue(KeyWebsiteName);
                ViewBag.News = await _rep.GetNewsList(lang, Convert.ToInt32(_common.GetConfigValue(ConstantStrings.KeyHotNewsLimit)));
                ViewBag.Products = await _rep.GetProductList(lang, Convert.ToInt32(_common.GetConfigValue(ConstantStrings.KeyHotProductLimit)));
                ViewBag.FAQ = await _rep.GetFAQList(lang, Convert.ToInt32(_common.GetConfigValue(ConstantStrings.KeyHotFAQLimit)));
                ViewBag.HomePageIntro = await _rep.GetHomePage("intro", lang);
                ViewBag.HomePageFeature = await _rep.GetHomePage("feature", lang);
                ViewBag.Customer = _repCustomer.GetProfile();
                if (!String.IsNullOrEmpty(_common.GetConfigValue(ConstantStrings.KeyHomeImage)))
                {
                    ViewBag.HomeImage = ConstantStrings.UrlConfigurationImages + _common.GetConfigValue(ConstantStrings.KeyHomeImage);

                }
                else
                {
                    ViewBag.HomeImage = "";
                }
                if (!String.IsNullOrEmpty(_common.GetConfigValue(ConstantStrings.HomeImageMobile)))
                {
                    ViewBag.HomeImageMobile = ConstantStrings.UrlConfigurationImages + _common.GetConfigValue(ConstantStrings.HomeImageMobile);

                }
                else
                {
                    ViewBag.HomeImageMobile = "";
                }
                if (!String.IsNullOrEmpty(_common.GetConfigValue(ConstantStrings.KeyFeatureImage)))
                {
                    ViewBag.FeatureImage = ConstantStrings.UrlConfigurationImages + _common.GetConfigValue(ConstantStrings.KeyFeatureImage);

                }
                else
                {
                    ViewBag.FeatureImage = "";
                }
                if (!String.IsNullOrEmpty(_common.GetConfigValue(ConstantStrings.KeyFAQImage)))
                {
                    ViewBag.FAQImage = ConstantStrings.UrlConfigurationImages + _common.GetConfigValue(ConstantStrings.KeyFAQImage);

                }
                else
                {
                    ViewBag.FAQImage = "";
                }

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = WebsiteName;
                _meta.OgType = OgTypeWebsite;
                ViewBag.Meta = _page.GetMeta(_meta);

                return View();
            }
            catch (Exception ex)
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }
    }
}
