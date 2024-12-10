using CMS.Areas.About.Models;
using CMS.Areas.Shared.Helper;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using CMS.Services.TranslateServices;
using DTO;
using DTO.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CMS.Areas.About.Controllers
{
    [Area("About")]
    public class AboutController : Controller
    {
        private string PageLimitAdmin = "";
        private readonly IAboutRepository _rep;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;
        private readonly ITranslateServices _translate;

        public AboutController(IAboutRepository rep,
                               IFileServices fileServices, ICommonServices common, ITranslateServices translate)
        {
            _rep = rep;
            _fileServices = fileServices;
            _common = common;
            _translate = translate;
            PageLimitAdmin = _common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
        }
        [CustomAuthorization("About", "VIEW")]
        public IActionResult Index(string editPid)
        {
            if (editPid != null)
            {
                ViewBag.EditPid = editPid;
            }
            else
            {
                ViewBag.EditPid = "";
            }
            return View();
        }
        public IActionResult Compose(string lang)
        {
            HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Compose");
        }
        [CustomAuthorization("About", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj)
        {
            AboutDetail objDetail = JsonConvert.DeserializeObject<AboutDetail>(stringObj);
            List<MultiLang_AboutDetail> objList = JsonConvert.DeserializeObject<List<MultiLang_AboutDetail>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Insert(objDetail, objList), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("About", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {
            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("About", "VIEW")]
        public JsonResult Edit(int Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("About", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj)
        {
            AboutDetail objDetail = JsonConvert.DeserializeObject<AboutDetail>(stringObj);
            List<MultiLang_AboutDetail> objList = JsonConvert.DeserializeObject<List<MultiLang_AboutDetail>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Update(objDetail, objList), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("About", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("About", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });

        }
        [CustomAuthorization("About", "EDIT")]
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
            return PartialView("Compose");
        }
        [CustomAuthorization("About", "ADD")]
        public JsonResult Coppy(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.Coppy(Pid), listData = _rep.LoadData(search) });

        }
        public JsonResult Up(long Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize; ;
            return Json(new { isError = _rep.Up(Pid), listData = _rep.LoadData(search) });
        }
        public JsonResult Down(long Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.Down(Pid), listData = _rep.LoadData(search) });
        }
        public JsonResult Preview(string stringObj, string stringMultiLangObj, IFormFile PicThumb)
        {
            var result = _rep.Preview(stringObj, stringMultiLangObj, PicThumb);
            if (result)
            {
                return Json(_translate.GetStringWithLangAdmin("preview.about", "vi"));
            }
            return Json("");
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
        public JsonResult SaveStatus(long pid, bool value, string type)
        {
            return Json(new { isError = _rep.SaveStatus(pid, value, type) });
        }
    }
}