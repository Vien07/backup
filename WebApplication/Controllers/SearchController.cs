using Microsoft.AspNetCore.Mvc;
using CMS.Repository;
using CMS.Services.TranslateServices;
using CMS.Services.WebsiteServices;
using DTO.Website;
using DTO;
using CMS.Services.CommonServices;
using CMS.Services.PageServices;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using DTO.SearchResult;
using X.PagedList;
using System.Collections.Generic;
using System.Linq;
using DTO.News;

namespace CMS.Controllers
{
    public class SearchController : Controller
    {
        private readonly IWebsiteServices _website;
        private readonly IPageServices _page;
        private readonly ISearch_Repository _rep;
        private readonly INews_Repository _repNews;
        private readonly ICommonServices _common;
        private readonly ITranslateServices _translate;
        private string OgTypeArticle = ConstantStrings.OgTypeArticle;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SearchController(ISearch_Repository rep, IWebsiteServices website, ICommonServices common, ITranslateServices translate, IPageServices page, IHttpContextAccessor httpContextAccessor, INews_Repository repNews)
        {
            _rep = rep;
            _repNews = repNews;
            _common = common;
            _website = website;
            _website.StartUp();
            _translate = translate;
            _page = page;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index(string q, string lang, int page)
        {
            try
            {
                if (string.IsNullOrEmpty(q))
                {
                    return View("~/Views/Home/Index.cshtml");
                }

                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                _httpContextAccessor.HttpContext.Session.SetString(ConstantStrings.WebsiteLang, lang);

                if (page < 1)
                {
                    page = 1;
                }


                ViewBag.Key = q;

                var data = await _rep.GetNewsList(lang, q);
                ViewBag.HighViewList = await _repNews.GetHighViewList(lang, 5);
                ViewBag.Advertisements = await _page.GetAdvertisements(ConstantStrings.NewsId);
                PagedList<NewsDto> dataPaging = new PagedList<NewsDto>(data, page, Convert.ToInt32(_common.GetConfigValue(ConstantStrings.KeyPageLimit)));

                ViewBag.Data = dataPaging.ToList();
                ViewBag.PageTotal = dataPaging.PageCount;
                ViewBag.CurrentPage = page;
                ViewBag.Banner = await _page.GetBanner(ConstantStrings.HomeId);
                MetaDto _meta = new MetaDto();


                _meta.PageTitle = _translate.GetString("search.search-result-for-keyword") + " \"" + q + "\"";
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