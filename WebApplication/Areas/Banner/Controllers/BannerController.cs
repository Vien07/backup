using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CMS.Areas.Shared.Helper;
using CMS.Areas.Banner.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;

namespace CMS.Areas.Banner.Controllers
{
    [Area("Banner")]
    public class BannerController : Controller
    {
        public string PageLimitAdmin = "";
        private readonly IBannerRepository _rep;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;

        public BannerController(IBannerRepository rep,
                               IFileServices fileServices, ICommonServices common)
        {
            _rep = rep;
            _fileServices = fileServices;
            _common = common;
            PageLimitAdmin = _common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
        }
        [CustomAuthorization("Banner", "VIEW")]
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
        [CustomAuthorization("Banner", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj, string listCates, IFormFile PicThumb)
        {
            Models.Banner objDetail = JsonConvert.DeserializeObject<Models.Banner>(stringObj);
            List<MultiLang_Banner> objList = JsonConvert.DeserializeObject<List<MultiLang_Banner>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Insert(objDetail, objList, listCates, PicThumb), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Banner", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {

            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("Banner", "VIEW")]
        public JsonResult Edit(int Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("Banner", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj, string listCates, IFormFile PicThumb)
        {
            Models.Banner objDetail = JsonConvert.DeserializeObject<Models.Banner>(stringObj);
            List<MultiLang_Banner> objList = JsonConvert.DeserializeObject<List<MultiLang_Banner>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Update(objDetail, objList, listCates, PicThumb), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Banner", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData(search) });
        }
        [CustomAuthorization("Banner", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData(search) });

        }
        [CustomAuthorization("Banner", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
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
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.Up(Pid), listData = _rep.LoadData(search) });
        }
        public JsonResult Down(long Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
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