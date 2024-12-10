using CMS.Repository;
using CMS.Services.CommonServices;
using CMS.Services.PageServices;
using CMS.Services.TranslateServices;
using CMS.Services.WebsiteServices;
using DTO;
using DTO.Website;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    public class TagKeyController : Controller
    {
        private readonly IWebsiteServices _website;
        private readonly IPageServices _page;
        private readonly ITagKey_Repository _rep;
        private readonly ICommonServices _common;
        private readonly ITranslateServices _translate;
        private string OgTypeArticle = ConstantStrings.OgTypeArticle;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TagKeyController(ITagKey_Repository rep, IWebsiteServices website, ICommonServices common, ITranslateServices translate, IPageServices page, IHttpContextAccessor httpContextAccessor)
        {
            _rep = rep;
            _common = common;
            _website = website;
            _website.StartUp();
            _translate = translate;
            _page = page;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index(string key, string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                ViewBag.Key = key;
                //var news = await _rep.GetListNews(lang, key);
                //ViewBag.News = news;

                var products = await _rep.GetListProduct(lang, key);
                ViewBag.Products = products;

                ViewBag.RootDomain = _common.GetConfigValue(ConstantStrings.KeyRootDomain);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.tags");
                _meta.OgType = OgTypeArticle;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View();
            }
            catch
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
