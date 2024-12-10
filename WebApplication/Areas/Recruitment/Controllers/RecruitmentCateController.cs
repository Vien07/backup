using System;
using System.Collections.Generic;
using CMS.Areas.Recruitment.Models;
using CMS.Areas.Shared.Helper;
using CMS.Services.CommonServices;
using CmsModels;
using DTO;
using DTO.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace CMS.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class RecruitmentCateController : Controller
    {
        public string PageLimitAdmin = "";
        private readonly IRecruitmentCateRepository _rep;
        private readonly ICommonServices _common;

        private readonly DBContext _dBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RecruitmentCateController(DBContext dBContext, IHttpContextAccessor httpContextAccessor, IRecruitmentCateRepository rep , ICommonServices common)
        {
            _common = common;

            _rep = rep;
            _dBContext = dBContext;
            PageLimitAdmin = _common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);

            _httpContextAccessor = httpContextAccessor;
        }
        [CustomAuthorization("RecruitmentCate", "VIEW")]
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
        [CustomAuthorization("RecruitmentCate", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj)
        {
            RecruitmentCate objDetail = JsonConvert.DeserializeObject<RecruitmentCate>(stringObj);
            List<MultiLang_RecruitmentCate> objList = JsonConvert.DeserializeObject<List<MultiLang_RecruitmentCate>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize; ;

            return Json(new { error = _rep.Insert(objDetail, objList), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("RecruitmentCate", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {
            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("RecruitmentCate", "VIEW")]
        public JsonResult Edit(long Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("RecruitmentCate", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj)
        {
            RecruitmentCate objDetail = JsonConvert.DeserializeObject<RecruitmentCate>(stringObj);
            List<MultiLang_RecruitmentCate> objList = JsonConvert.DeserializeObject<List<MultiLang_RecruitmentCate>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize; ;

            return Json(new { error = _rep.Update(objDetail, objList), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("RecruitmentCate", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize; ;
            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("RecruitmentCate", "DELETE")]
        public JsonResult DeleteAll(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize; ;
            return Json(new { isError = _rep.DeleteAll(Pid), jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("RecruitmentCate", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize;

            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });

        }
        [CustomAuthorization("RecruitmentCate", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize; ;
            return Json(new { isError = _rep.Enable(Pid, Enabled), listData = _rep.LoadData(search) });

        }
        public ActionResult OpenAddModal(string lang)
        {
            _httpContextAccessor.HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Modal");
        }
        [CustomAuthorization("RecruitmentCate", "ADD")]
        public JsonResult Coppy(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize; ;
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
            search.PageNumber = ConstantStrings.DefaultPageSize; ;
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
        #endregion
    }
}