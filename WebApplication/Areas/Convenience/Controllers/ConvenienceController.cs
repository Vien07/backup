using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CMS.Areas.Shared.Helper;
using CMS.Areas.Convenience.Models;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using DTO;

namespace CMS.Areas.Convenience.Controllers
{
    [Area("Convenience")]
    public class ConvenienceController : Controller
    {
        public string PageLimitAdmin = "";
        private readonly IConvenienceRepository _rep;
        private readonly ICommonServices _core;
        private readonly DBContext _dBContext;

        public ConvenienceController(DBContext dBContext, IConvenienceRepository rep, ICommonServices core)
        {
            PageLimitAdmin = core.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
            _dBContext = dBContext;
            _rep = rep;
            _core = core;
        }
        [CustomAuthorization("Convenience", "VIEW")]
        public IActionResult Index()
        {

            //Startup._httpContextAccessor.HttpContext.Session.SetString("LangCompose", "");
            //var a =Startup._httpContextAccessor.HttpContext.Session.GetString("UserAvatar");
            return View();
        }
        public IActionResult Modal(string lang)
        {
            HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Modal");
        }
        #region action Detail
        [CustomAuthorization("Convenience", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj, IFormFile PicThumb)
        {
            Models.Convenience objDetail = JsonConvert.DeserializeObject<Models.Convenience>(stringObj);
            List<MultiLang_Convenience> objList = JsonConvert.DeserializeObject<List<MultiLang_Convenience>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Insert(objDetail, objList, PicThumb), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Convenience", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {

            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("Convenience", "VIEW")]
        public JsonResult Edit(long Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("Convenience", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj, IFormFile PicThumb)
        {
            Models.Convenience objDetail = JsonConvert.DeserializeObject<Models.Convenience>(stringObj);
            List<MultiLang_Convenience> objList = JsonConvert.DeserializeObject<List<MultiLang_Convenience>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Update(objDetail, objList, PicThumb), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Convenience", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData(search) });
        }
        [CustomAuthorization("Convenience", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData(search) });

        }
        [CustomAuthorization("Convenience", "EDIT")]
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