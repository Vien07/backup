using System;
using System.Collections.Generic;
using CMS.Areas.FAQ.Models;
using CMS.Areas.Shared.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DTO;
using CMS.Services.CommonServices;
using DTO.Common;

namespace CMS.Areas.FAQ.Controllers
{
    [Area("FAQ")]
    public class FAQCateController : Controller
    {
        private string PageLimitAdmin = "";
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;

        private readonly IFAQCateRepository _rep;
        private readonly ICommonServices _common;

        private readonly DBContext _dBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FAQCateController(DBContext dBContext, IHttpContextAccessor httpContextAccessor, IFAQCateRepository rep, ICommonServices common)
        {
            _common = common;

            _rep = rep;
            _dBContext = dBContext;
            PageLimitAdmin = _common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);

            _httpContextAccessor = httpContextAccessor;
        }
        [CustomAuthorization("FAQCate", "VIEW")]
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
        [CustomAuthorization("FAQCate", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj)
        {
            FAQCate objDetail = JsonConvert.DeserializeObject<FAQCate>(stringObj);
            List<MultiLang_FAQCate> objList = JsonConvert.DeserializeObject<List<MultiLang_FAQCate>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;

            return Json(new { error = _rep.Insert(objDetail, objList), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("FAQCate", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {
            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("FAQCate", "VIEW")]
        public JsonResult Edit(long Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("FAQCate", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj)
        {
            FAQCate objDetail = JsonConvert.DeserializeObject<FAQCate>(stringObj);
            List<MultiLang_FAQCate> objList = JsonConvert.DeserializeObject<List<MultiLang_FAQCate>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;

            return Json(new { error = _rep.Update(objDetail, objList), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("FAQCate", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize;
            var data = _rep.Delete(Pid);
            return Json(new { isError = data.value, messError = data.messError, jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("FAQCate", "DELETE")]
        public JsonResult DeleteAll(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            var data = _rep.DeleteAll(Pid);
            return Json(new { isError = data.value, messError = data.messError, jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("FAQCate", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize;
            var data = _rep.Delete(Pid);
            return Json(new { isError = data.value, messError = data.messError, jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("FAQCate", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Enable(Pid, Enabled), listData = _rep.LoadData(search) });

        }
        public ActionResult OpenAddModal(string lang)
        {
            _httpContextAccessor.HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Modal");
        }
        [CustomAuthorization("FAQCate", "ADD")]
        public JsonResult Coppy(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Coppy(Pid), listData = _rep.LoadData(search) });

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
        public JsonResult MoveRow(long from, long to)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.MoveRow(from, to), list = _rep.LoadData(search) });
        }
        public JsonResult LoadByCate(string cate)
        {
            TempData["cate"] = cate;
            return Json("/b-admin/Course/");
        }
        public JsonResult UpdateOrder(long Pid, int Order)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.UpdateOrder(Pid, Order), list = _rep.LoadData(search) });
        }
        #endregion
    }
}