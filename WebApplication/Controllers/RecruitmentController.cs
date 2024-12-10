using System;

using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;

using CMS.Repository;
using CmsModels;
using CMS.Services.WebsiteServices;
using CMS.Services.CommonServices;
using CMS.Services.PageServices;
using DTO.Website;
using DTO.Recruitment;
using CMS.Services.TranslateServices;
using CMS.Services.EmailServices;
using DTO;
using System.Threading.Tasks;

namespace CMS.Controllers
{
    public class RecruitmentController : Controller
    {
        public string PageLimit = "0";
        private string Keyrecaptcha = ConstantStrings.Keyrecaptcha;

        private readonly IEmailServices _mailSrv;
        private readonly IWebsiteServices _website;
        private readonly ITranslateServices _translate;
        private readonly IRecruitment_Repository _rep;
        private readonly ICommonServices _common;
        private readonly IContact_Repository _repContact;
        private readonly IPageServices _page;

        public RecruitmentController(IRecruitment_Repository rep, IContact_Repository repContact, IWebsiteServices website, ICommonServices common, IPageServices page, ITranslateServices translate, IEmailServices mailSrv)
        {
            _mailSrv = mailSrv;
            _rep = rep;
            _translate = translate;
            _common = common;
            _website = website;
            _repContact = repContact;
            _page = page;
            PageLimit = _common.GetConfigValue(ConstantStrings.KeyPageLimit);
            _website.StartUp();
        }
        public async Task<IActionResult> Index(string lang, int page, string key)
        {
            try
            {

                ViewBag.PageLimit = PageLimit;
                dynamic data = _rep.GetList(lang, page, key);
                ViewBag.Data = data.list;
                ViewBag.Paging = data.paging;
                ViewBag.PageTotal = data.paging.pageTotal;
                ViewBag.CurrentPage = data.paging.currentPage;
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.RecruitmentId);
                ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.RecruitmentId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.RecruitmentId);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = _translate.GetString("menu.recruitment");
                _meta.OgType = ConstantStrings.OgTypeArticle;
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
                string _host = "//" + HttpContext.Request.Host.Host.ToString();
                ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.RecruitmentId);
                var currCate = _rep.GetCate(cate, lang);
                ViewBag.CurrentCate = currCate;
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.RecruitmentId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.RecruitmentId);

                dynamic data = _rep.GetListBySlug(lang, page, cate);
                ViewBag.CateSlug = cate;
                ViewBag.Cate = _rep.GetCate(lang);
                ViewBag.Data = data.list;
                ViewBag.Paging = data.paging;
                ViewBag.PageTotal = data.paging.pageTotal;
                ViewBag.CurrentPage = data.paging.currentPage;
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = currCate.Title;
                _meta.PageUrl = _translate.GetUrl("url.recruitment") + cate + "/";
                _meta.OgType = ConstantStrings.OgTypeArticle;
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
                ViewBag.PreNext = _rep.GetPreNextRecruit(slug, lang);
                ViewBag.Recaptcha = _common.GetConfigValue(Keyrecaptcha);
                //ViewBag.Data = _rep.GetRecruitment(slug, lang);
                ViewBag.reCapchaSiteKey = _common.GetConfigValue(ConstantStrings.KeyreCapchaSiteKey);
                ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.RecruitmentId);
                ViewBag.Data = await _rep.GetRecruit(slug, lang);
                string _host = /*HttpContext.Request.Scheme+*/ "//" + HttpContext.Request.Host.Host.ToString();
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.RecruitmentId);
                ViewBag.Popup = await _page.GetPopup(ConstantStrings.RecruitmentId);

                ViewBag.RelateList = _rep.GetRelateList(slug, lang);
                MetaDto _meta = new MetaDto();
                _meta.PageTitle = ViewBag.Data.Title;
                _meta.Description = ViewBag.Data.Description;
                _meta.ImageUrl = /*_host +*/ ViewBag.Data.PicFull;
                _meta.Keywords = ViewBag.Data.TagKey;
                _meta.PageUrl = _translate.GetUrl("url.recruitment") + ViewBag.Data.Slug + ".html";
                _meta.OgType = ConstantStrings.OgTypeArticle;
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
        public JsonResult SendCV(CVDto model)
        {
            var result = _rep.SendCV(model);
            if (result)
            {
                _mailSrv.SendRecruitToAdmin(model);
                _mailSrv.SendRecruitToCus(model);
            }
            return Json(result);
        }
    }
}