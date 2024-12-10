using System;
using Microsoft.AspNetCore.Mvc;
using CMS.Services;
using Microsoft.AspNetCore.Http;
using CMS.Repository;
using CMS.Services.TranslateServices;
using CMS.Services.WebsiteServices;
using CMS.Services.PageServices;
using DTO.Website;
using DTO;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    public class AboutController : Controller
    {
        private readonly IWebsiteServices _website;
        private readonly IPageServices _page;
        private readonly ITranslateServices _translate;
        private readonly IAbout_Repository _rep;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string OgTypeArticle = ConstantStrings.OgTypeArticle;

        public AboutController(IAbout_Repository rep, IWebsiteServices website, ITranslateServices translate, IPageServices page, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _rep = rep;
            _website = website;
            _translate = translate;
            _page = page;
            _website.StartUp();
        }
        public async Task<IActionResult> Index(string lang, int page)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);

                ViewBag.Banner = await _page.GetBanner(ConstantStrings.AboutId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.AboutId);
                var slug = await _rep.GetSlugDefault(lang);
                if (!string.IsNullOrEmpty(slug))
                {
                    string url = _translate.GetUrl("url.about") + slug + ".html";
                    return Redirect(url);
                }
                return View();
            }
            catch (Exception)
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }

        }
        public async Task<IActionResult> Detail(string slug, string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);

                var data = await _rep.GetAbout(slug, lang);
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.AboutId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.AboutId);

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = data.Title;
                _meta.ImageUrl = data.OrgImages;
                _meta.Keywords = data.TagKey;
                _meta.OgType = OgTypeArticle;
                _meta.PageUrl = _translate.GetUrl("url.about") + data.Slug + ".html";
                ViewBag.Meta = _page.GetMeta(_meta);

                ViewBag.Data = data;
                return View();
            }
            catch (Exception)
            {
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = "Page Not Found";
                _meta.Is404Page = true;
                ViewBag.Meta = _page.GetMeta(_meta);
                return View("~/Views/Error/Index.cshtml");
            }
        }
        public async Task<IActionResult> Preview()
        {
            try
            {
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.AboutId);
                var data = _rep.GetAboutPreview();
                ViewBag.Data = data;
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.about");
                _meta.OgType = OgTypeArticle;

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