using CMS.Repository;
using CMS.Services.CommonServices;
using CMS.Services.PageServices;
using CMS.Services.TranslateServices;
using CMS.Services.WebsiteServices;
using DTO;
using DTO.Gallery;
using DTO.Website;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CMS.Controllers
{
    public class GalleryController : Controller
    {
        private string PageLimit = "0";
        private readonly IWebsiteServices _website;
        private readonly IGallery_Repository _rep;
        private readonly ICommonServices _common;
        private readonly ITranslateServices _translate;
        private readonly IPageServices _page;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string KeyWebsiteName = ConstantStrings.KeyWebsiteName;
        private string OgTypeWebsite = ConstantStrings.OgTypeWebsite;
        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string OgTypeArticle = ConstantStrings.OgTypeArticle;
        private string KeySEOConfig = ConstantStrings.KeySEOConfig;

        public GalleryController(IGallery_Repository rep, IWebsiteServices website, ICommonServices common, ITranslateServices translate, IPageServices page, IHttpContextAccessor httpContextAccessor)
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

                ViewBag.Banner = await _page.GetBanner(ConstantStrings.GalleryId);
                //ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.GalleryId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.GalleryId);
                ViewBag.GalleryCateList = await _rep.GetCateList(lang);

                var data = await _rep.GetList(lang, page);
                PagedList<GalleryDto> dataPaging = new PagedList<GalleryDto>(data, page, Convert.ToInt32(PageLimit));
                ViewBag.Data = dataPaging.ToList();
                ViewBag.PageTotal = dataPaging.PageCount;
                ViewBag.CurrentPage = page;

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.gallery");
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

                ViewBag.Banner = await _page.GetBanner(ConstantStrings.GalleryId);
                //ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.GalleryId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.GalleryId);
                ViewBag.GalleryCateList = await _rep.GetCateList(lang);
                //ViewBag.HighViewList = await _rep.GetHighViewList(lang, 4);

                var data = await _rep.GetListDict(lang, 3, 6);
                ViewBag.Data = data;

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.gallery");
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

                ViewBag.Banner = await _page.GetBanner(ConstantStrings.GalleryId);
                //ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.GalleryId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.GalleryId);

                var currCate = await _rep.GetCate(cate, lang);
                ViewBag.CurrentCate = currCate;
                ViewBag.Cate = await _rep.GetCateList(lang);

                var data = await _rep.GetListBySlug(lang, page, cate);
                PagedList<GalleryDto> dataPaging = new PagedList<GalleryDto>(data, page, Convert.ToInt32(PageLimit));
                ViewBag.Data = dataPaging.ToList();
                ViewBag.PageTotal = dataPaging.PageCount;
                ViewBag.CurrentPage = page;

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = currCate.Title;
                _meta.PageUrl = _translate.GetUrl("url.gallery") + cate + "/";
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

                ViewBag.Banner = await _page.GetBanner(ConstantStrings.GalleryId);
                //ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.GalleryId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.GalleryId);
                //ViewBag.RelateList = await _rep.GetRelateList(slug, lang, Convert.ToInt32(_common.GetConfigValue(ConstantStrings.KeyRelateGalleryLimit)));
                //ViewBag.CateList = await _rep.GetCateList(lang);
                ViewBag.HowImageListShow = _common.GetConfigValue(ConstantStrings.KeyHowImageListShow);
                ViewBag.PositionImageListShow = _common.GetConfigValue(ConstantStrings.KeyPositionImageListShow);

                var data = await _rep.GetGallery(slug, lang);
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
                _meta.PageUrl = _translate.GetUrl("url.gallery") + data.Slug + ".html";
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
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.GalleryId);
                ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.GalleryId);

                var data = _rep.GetGalleryPreview();
                ViewBag.Data = data;
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.gallery");
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