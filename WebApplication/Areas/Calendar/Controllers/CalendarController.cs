using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CMS.Areas.Shared.Helper;
using CMS.Areas.Calendar.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;

namespace CMS.Areas.Calendar.Controllers
{
    [Area("Calendar")]
    public class CalendarController : Controller
    {
        private string PageLimitAdmin = "";

        private readonly ICalendarRepository _rep;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;
        private readonly DBContext _dBContext;

        private int DefaultPageSize = ConstantStrings.DefaultPageSize;
        private string Thumb = ConstantStrings.Thumb;
        private string DefaultLang = ConstantStrings.DefaultLang;

        public CalendarController(DBContext dBContext, ICalendarRepository rep,
                               IFileServices fileServices, ICommonServices common)
        {
            _dBContext = dBContext;
            _rep = rep;
            _fileServices = fileServices;
            _common = common;
            PageLimitAdmin = _common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
        }
        [CustomAuthorization("Calendar", "VIEW")]
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
        [CustomAuthorization("Calendar", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj)
        {
            Models.Calendar objDetail = JsonConvert.DeserializeObject<Models.Calendar>(stringObj);
            List<MultiLang_Calendar> objList = JsonConvert.DeserializeObject<List<MultiLang_Calendar>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;

            return Json(new { error = _rep.Insert(objDetail, objList), list = _rep.LoadData() });
        }
        //[CustomAuthorization("Calendar", "VIEW")]
        //public JsonResult LoadData(SearchDto search)
        //{

        //    var data = _rep.LoadData(search);
        //    return Json(data);
        //}
        [CustomAuthorization("Calendar", "VIEW")]
        public JsonResult LoadData()
        {

            var data = _rep.LoadData();
            return Json(data);
        }
        [CustomAuthorization("Calendar", "VIEW")]
        public JsonResult Edit(long Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("Calendar", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj)
        {
           Models.Calendar objDetail = JsonConvert.DeserializeObject<Models.Calendar>(stringObj);
            List<MultiLang_Calendar> objList = JsonConvert.DeserializeObject<List<MultiLang_Calendar>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;

            return Json(new { error = _rep.Update(objDetail, objList), list = _rep.LoadData() });
        }
        [CustomAuthorization("Calendar", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData() });
        }
        [CustomAuthorization("Calendar", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize;

            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData() });

        }
        [CustomAuthorization("Calendar", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Enable(Pid, Enabled), listData = _rep.LoadData() });

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
            return Json(new { isError = _rep.Up(Pid), listData = _rep.LoadData() });
        }
        public JsonResult Down(long Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Down(Pid), listData = _rep.LoadData() });
        }
        public JsonResult UpdateOrder(long Pid, int Order)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.UpdateOrder(Pid, Order), list = _rep.LoadData() });
        }
        public JsonResult MoveRow(long from, long to)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.MoveRow(from, to), list = _rep.LoadData() });
        }
        #endregion
    }
}