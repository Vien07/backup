using CMS.Repository;
using DTO.Cart;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using CMS.Services.PageServices;
using CMS.Services.CommonServices;
using DTO.Website;
using CMS.Services.TranslateServices;
using Microsoft.AspNetCore.Http;

namespace CMS.Controllers
{
    public class CartController : Controller
    {
        private readonly ICart_Repository _rep;
        private readonly ICustomer_Repository _repCustomer;
        private readonly ITranslateServices _translate;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IPageServices _page;
        private readonly ICommonServices _common;
        private string KeyreCapchaSiteKey = ConstantStrings.KeyreCapchaSiteKey;
        private string Keyrecaptcha = ConstantStrings.Keyrecaptcha;

        public CartController(ICart_Repository rep, IPageServices page, ICommonServices common, ICustomer_Repository repCustomer, ITranslateServices translate, IHttpContextAccessor httpContextAccessor)
        {
            _rep = rep;
            _page = page;
            _common = common;
            _repCustomer = repCustomer;
            _translate = translate;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.CartId);
                ViewBag.Customer = _repCustomer.GetProfile();
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.cart");
                _meta.OgType = ConstantStrings.OgTypeWebsite;
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

        public async Task<IActionResult> CheckOut(string lang)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                var customer = _repCustomer.GetProfile();
                if (customer.Pid == 0)
                {
                    return Redirect("dang-nhap.html");
                }
                ViewBag.Customer = customer;
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.NewsId);
                ViewBag.InfoPayment = _common.GetConfigValue(ConstantStrings.KeyiBankingInfo);
                ViewBag.DisplayiBanking = _common.GetConfigValue(ConstantStrings.KeyDisplayiBanking);
                ViewBag.reCapchaSiteKey = _common.GetConfigValue(KeyreCapchaSiteKey);
                ViewBag.Recaptcha = _common.GetConfigValue(Keyrecaptcha);

                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.check-out");
                _meta.OgType = ConstantStrings.OgTypeWebsite;
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
        public async Task<IActionResult> CheckOutSuccess(string lang, string orderId)
        {
            try
            {
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                var customer = _repCustomer.GetProfile();
                var order = await _rep.GetOrder(orderId);

                if (customer.Pid != order.CustomerId)
                {
                    return Redirect("/");
                }
                ViewBag.Customer = customer;
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.NewsId);
                ViewBag.Order = order;
                ViewBag.InfoPayment = _common.GetConfigValue(ConstantStrings.KeyiBankingInfo);
                ViewBag.DisplayiBanking = _common.GetConfigValue(ConstantStrings.KeyDisplayiBanking);
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.check-out-success");
                _meta.OgType = ConstantStrings.OgTypeWebsite;
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
        public async Task<JsonResult> GetCart(string cartStr, string lang)
        {
            return Json(await _rep.GetCartInfo(cartStr, lang));
        }
        public async Task<JsonResult> SaveOrder(string cartStr, string lang, OrderInformation information)
        {
            var result = await _rep.SaveOrder(cartStr, lang, information);
            return Json(new { IsError = result.IsError, OrderId = result.OrderId });
        }

    }
}
