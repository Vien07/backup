using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CMS.Repository;
using CMS.Services.TranslateServices;
using CMS.Services.WebsiteServices;
using CMS.Services.PageServices;
using DTO.Website;
using DTO;
using CMS.Services.CommonServices;
using DTO.Product;
using System.Threading.Tasks;
using X.PagedList;
using System.Linq;
using System.Globalization;

namespace CMS.Controllers
{
    public class ProductController : Controller
    {
        private string PageLimit = "0";
        private readonly IWebsiteServices _website;
        private readonly IProduct_Repository _rep;
        private readonly ICustomer_Repository _repCustomer;
        private readonly ICommonServices _common;
        private readonly ITranslateServices _translate;
        private readonly IPageServices _page;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string KeyWebsiteName = ConstantStrings.KeyWebsiteName;
        private string OgTypeWebsite = ConstantStrings.OgTypeWebsite;
        private string KeyPageLimit = ConstantStrings.KeyPageLimit;
        private string OgTypeArticle = ConstantStrings.OgTypeArticle;
        private string OgTypeProduct = ConstantStrings.OgTypeProduct;
        private string KeySEOConfig = ConstantStrings.KeySEOConfig;

        public ProductController(IProduct_Repository rep, IWebsiteServices website, ICommonServices common, ITranslateServices translate, IPageServices page, IHttpContextAccessor httpContextAccessor, ICustomer_Repository repCustomer)
        {
            _rep = rep;
            _repCustomer = repCustomer;
            _common = common;
            _website = website;
            _translate = translate;
            _page = page;
            PageLimit = _common.GetConfigValue(KeyPageLimit);
            _website.StartUp();
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IActionResult> Index(string lang, int page, string sortby)
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
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.ProductId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.ProductId);
                var data = await _rep.GetList(lang, page, sortby);
                ViewBag.Customer = _repCustomer.GetProfile();
                ViewBag.Data = data;


                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.product");
                _meta.OgType = OgTypeProduct;

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
        public async Task<IActionResult> Cate(string lang, int page, string cate, string sortby)
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
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.ProductId);
                //ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.ProductId);
                ViewBag.ProductCateList = await _rep.GetCateList(lang);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.ProductId);

                var currCate = await _rep.GetCate(cate, lang);
                ViewBag.CurrentCate = currCate;

                var data = await _rep.GetListBySlug(lang, page, cate, sortby);
                PagedList<ProductDto> dataPaging = new PagedList<ProductDto>(data, page, Convert.ToInt32(PageLimit));
                ViewBag.Data = dataPaging.ToList();
                ViewBag.PageTotal = dataPaging.PageCount;
                ViewBag.CurrentPage = page;

                ViewBag.Count = _common.ConvertFormatMoney(data.Count);

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = currCate.Title;
                _meta.PageUrl = _translate.GetUrl("url.product") + cate + "/";
                _meta.OgType = OgTypeProduct;
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

                ViewBag.Banner = await _page.GetBanner(ConstantStrings.ProductId);
                //ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.ProductId);
                ViewBag.RelateList = await _rep.GetRelateList(slug, lang, Convert.ToInt32(_common.GetConfigValue(ConstantStrings.KeyRelateProductLimit)));
                //ViewBag.CateList = await _rep.GetCateList(lang);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.ProductId);

                var data = await _rep.GetProduct(slug, lang);
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
                _meta.PageUrl = _translate.GetUrl("url.product") + data.Slug + ".html";
                _meta.OgType = OgTypeProduct;
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
        public async Task<JsonResult> ChangePrice(int optionId, long productId)
        {
            return Json(await _rep.ChangePrice(optionId, productId));
        }

        [HttpPost]
        public async Task<JsonResult> InsertComment(int productId, int customerId, string message, int? parentId)
        {
            try
            {
                return Json(await _rep.InsertComment(productId, customerId, message, parentId));
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        public async Task<IActionResult> Preview()
        {
            try
            {
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.ProductId);
                ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.ProductId);

                var data = _rep.GetProductPreview();
                ViewBag.Data = data;
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.news");
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