using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CMS.Areas.Shared.Helper;
using CMS.Areas.HomePage.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;

namespace CMS.Areas.HomePage.Controllers
{
    [Area("HomePage")]
    public class HomePageFeatureController : Controller
    {
        private string PageLimitAdmin = "";

        private readonly IHomePageFeatureRepository _rep;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;

        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;

        public HomePageFeatureController(IHomePageFeatureRepository rep,
                               IFileServices fileServices, ICommonServices common)
        {
            _rep = rep;
            _fileServices = fileServices;
            _common = common;
            PageLimitAdmin = _common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
        }
        [CustomAuthorization("HomePage", "VIEW")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Modal(string lang)
        {
            HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Modal");
        }
        #region action Detail
        [CustomAuthorization("HomePage", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj, IFormFile PicThumb)
        {
            Models.HomePage objDetail = JsonConvert.DeserializeObject<Models.HomePage>(stringObj);
            List<MultiLang_HomePage> objList = JsonConvert.DeserializeObject<List<MultiLang_HomePage>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;

            return Json(new { error = _rep.Insert(objDetail, objList, PicThumb), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("HomePage", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {

            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("HomePage", "VIEW")]
        public JsonResult Edit(long Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("HomePage", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj, IFormFile PicThumb)
        {
            Models.HomePage objDetail = JsonConvert.DeserializeObject<Models.HomePage>(stringObj);
            List<MultiLang_HomePage> objList = JsonConvert.DeserializeObject<List<MultiLang_HomePage>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;

            return Json(new { error = _rep.Update(objDetail, objList, PicThumb), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("HomePage", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData(search) });
        }
        [CustomAuthorization("HomePage", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize;

            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData(search) });

        }
        [CustomAuthorization("HomePage", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Enable(Pid, Enabled), listData = _rep.LoadData(search) });
        }
        public ActionResult OpenAddModal(string lang)
        {
            HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Modal");
        }
        public JsonResult Up(long Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Up(Pid), listData = _rep.LoadData(search) });
        }
        public JsonResult Down(long Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Down(Pid), listData = _rep.LoadData(search) });
        }
        public JsonResult UpdateOrder(long Pid, int Order)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.UpdateOrder(Pid, Order), list = _rep.LoadData(search) });
        }
        public JsonResult MoveRow(long from, long to)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.MoveRow(from, to), list = _rep.LoadData(search) });
        }
        #endregion
    }
}