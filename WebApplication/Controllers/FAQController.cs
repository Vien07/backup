using CMS.Repository;
using CMS.Services.CommonServices;
using CMS.Services.PageServices;
using CMS.Services.TranslateServices;
using CMS.Services.WebsiteServices;
using DTO;
using DTO.FAQ;
using DTO.Website;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CMS.Controllers
{
    public class FAQController : Controller
    {
        private string PageLimit = "0";
        private readonly IWebsiteServices _website;
        private readonly IFAQ_Repository _rep;
        private readonly ICommonServices _common;
        private readonly ITranslateServices _translate;
        private readonly IPageServices _page;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string KeyWebsiteName = ConstantStrings.KeyWebsiteName;
        private string OgTypeWebsite = ConstantStrings.OgTypeWebsite;
        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string OgTypeArticle = ConstantStrings.OgTypeArticle;
        private string KeySEOConfig = ConstantStrings.KeySEOConfig;

        public FAQController(IFAQ_Repository rep, IWebsiteServices website, ICommonServices common, ITranslateServices translate, IPageServices page, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _rep = rep;
            _common = common;
            _website = website;
            _translate = translate;
            _page = page;
            PageLimit = _common.GetConfigValue(KeyPageLimit);
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

                if (page < 1)
                {
                    page = 1;
                }

                ViewBag.PageLimit = PageLimit;

                ViewBag.Banner = await _page.GetBanner(ConstantStrings.FAQId);
                //ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.FAQId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.FAQId);
                //ViewBag.FAQCateList = await _rep.GetCateList(lang);

                var data = await _rep.GetList(lang, page);
                //PagedList<FAQDto> dataPaging = new PagedList<FAQDto>(data, page, Convert.ToInt32(PageLimit));
                //ViewBag.Data = dataPaging.ToList();
                //ViewBag.PageTotal = dataPaging.PageCount;
                //ViewBag.CurrentPage = page;

                ViewBag.Data = data;

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.faq");
                _meta.OgType = OgTypeArticle;

                ViewBag.Meta = _page.GetMeta(_meta);
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
        public async Task<IActionResult> ListWithCate(string lang, int page)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);

                if (page < 1)
                {
                    page = 1;
                }

                ViewBag.PageLimit = PageLimit;

                ViewBag.Banner = await _page.GetBanner(ConstantStrings.FAQId);
                ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.FAQId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.FAQId);
                ViewBag.FAQCateList = await _rep.GetCateList(lang);
                ViewBag.HighViewList = await _rep.GetHighViewList(lang, 4);

                var data = await _rep.GetListDict(lang, 3, 6);
                ViewBag.Data = data;

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.faq");
                _meta.OgType = OgTypeArticle;

                ViewBag.Meta = _page.GetMeta(_meta);
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
        public async Task<IActionResult> Cate(string lang, int page, string cate)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);

                if (page < 1)
                {
                    page = 1;
                }

                ViewBag.PageLimit = PageLimit;

                ViewBag.Banner = await _page.GetBanner(ConstantStrings.FAQId);
                ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.FAQId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.FAQId);

                var currCate = await _rep.GetCate(cate, lang);
                ViewBag.CurrentCate = currCate;
                ViewBag.Cate = await _rep.GetCateList(lang);

                var data = await _rep.GetListBySlug(lang, page, cate);
                PagedList<FAQDto> dataPaging = new PagedList<FAQDto>(data, page, Convert.ToInt32(PageLimit));
                ViewBag.Data = dataPaging.ToList();
                ViewBag.PageTotal = dataPaging.PageCount;
                ViewBag.CurrentPage = page;

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = currCate.Title;
                _meta.PageUrl = _translate.GetUrl("url.faq") + cate + "/";
                _meta.OgType = OgTypeArticle;
                ViewBag.Meta = _page.GetMeta(_meta);
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

                ViewBag.Banner = await _page.GetBanner(ConstantStrings.FAQId);
                ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.FAQId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.FAQId);
                ViewBag.RelateList = await _rep.GetRelateList(slug, lang, Convert.ToInt32(_common.GetConfigValue(ConstantStrings.KeyRelateFAQLimit)));
                ViewBag.CateList = await _rep.GetCateList(lang);

                var data = await _rep.GetFAQ(slug, lang);
                ViewBag.Data = data;

                MetaDto _meta = new MetaDto();

                if (_common.GetConfigValue(KeySEOConfig) != "on")
                {
                    _meta.PageTitle = data.Title;
                    _meta.Description = data.Description;
                }
                else
                {
                    _meta.PageTitle = data.TitleSEO;
                    _meta.Description = data.DescriptionSEO;
                }

                _meta.ImageUrl = data.ImageMeta;
                _meta.Keywords = data.TagKey;
                _meta.PageUrl = _translate.GetUrl("url.faq") + data.Slug + ".html";
                _meta.OgType = OgTypeArticle;
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
        public async Task<IActionResult> Preview()
        {
            try
            {
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.FAQId);
                ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.FAQId);

                var data = _rep.GetFAQPreview();
                ViewBag.Data = data;
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.faq");
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