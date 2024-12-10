using CMS.Areas.Contact.Models;
using CMS.Repository;
using CMS.Services.CommonServices;
using CMS.Services.EmailServices;
using CMS.Services.PageServices;
using CMS.Services.TranslateServices;
using CMS.Services.WebsiteServices;
using DTO;
using DTO.Website;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContact_Repository _rep;
        private readonly ICommonServices _common;
        private readonly IWebsiteServices _website;
        private readonly IPageServices _page;
        private readonly ITranslateServices _translate;
        private readonly IEmailServices _mailSrv;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string KeyShareFacebook = ConstantStrings.KeyShareFacebook;
        private string DefaultLang = ConstantStrings.DefaultLang;
        private string KeyShareTwitter = ConstantStrings.KeyShareTwitter;
        private string KeyShareLinkedin = ConstantStrings.KeyShareLinkedin;
        private string KeyShareInstagram = ConstantStrings.KeyShareInstagram;
        private string KeyreCapchaSiteKey = ConstantStrings.KeyreCapchaSiteKey;
        private string KeyShareYoutube = ConstantStrings.KeyShareYoutube;
        private string KeySharePinterest = ConstantStrings.KeySharePinterest;

        private string KeyRootDomain = ConstantStrings.KeyRootDomain;

        private string Keyrecaptcha = ConstantStrings.Keyrecaptcha;
        private string KeyEmailAdmin = ConstantStrings.KeyEmailAdmin;
        private string OgTypeArticle = ConstantStrings.OgTypeArticle;

        public ContactController(IEmailServices mailSrv, ICommonServices common, IContact_Repository rep, IWebsiteServices website, ITranslateServices translate, IPageServices page, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _rep = rep;
            _translate = translate;
            _common = common;
            _mailSrv = mailSrv;
            _page = page;
            _website = website;
            _website.StartUp();
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
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);
                ViewBag.Lang = lang;
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.ContactId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.ContactId);
                ViewBag.ContactInfo = _rep.GetContactInfo(lang);


                //ViewBag.ShareFacebook = _common.GetConfigValue(KeyShareFacebook);
                //ViewBag.ShareTwitter = _common.GetConfigValue(KeyShareTwitter);
                //ViewBag.ShareYoutube = _common.GetConfigValue(KeyShareYoutube);
                //ViewBag.ShareLinkedin = _common.GetConfigValue(KeyShareLinkedin);
                //ViewBag.ShareInstagram = _common.GetConfigValue(KeyShareInstagram);
                //ViewBag.SharePinterest = _common.GetConfigValue(KeySharePinterest);
                //ViewBag.Domain = _common.GetConfigValue(KeyRootDomain);
                ViewBag.RecaptchaSiteKey = _common.GetConfigValue(KeyreCapchaSiteKey);
                ViewBag.Recaptcha = _common.GetConfigValue(Keyrecaptcha);
                //ViewBag.EmailAdmin = _common.GetConfigValue(KeyEmailAdmin);

                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.contact");
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
        #region Action
        public JsonResult SendContact(ContactList data)
        {
            _mailSrv.SendToCus(data);
            _mailSrv.SendToAdmin(data);
            return Json(new { isError = _rep.SaveContact(data) });
        }
        public JsonResult SendEnquire(EnquireList data)
        {
            _mailSrv.EnquireToCus(data);
            _mailSrv.EnquireToAdmin(data);
            return Json(new { isError = _rep.SaveEnquire(data) });
        }
        #endregion
    }
}