﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CMS.Areas.Shared.Helper;
using CMS.Areas.Feature.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using CMS.Services.TranslateServices;

namespace CMS.Areas.Feature.Controllers
{
    [Area("Feature")]
    public class FeatureController : Controller
    {
        private readonly IFeatureRepository _rep;
        private readonly ITranslateServices _translate;
        private readonly ICommonServices _common;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string PageLimitAdmin = "";
        private string DateFormat = "";
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;

        public FeatureController(IHttpContextAccessor httpContextAccessor, IFeatureRepository rep,
                            ICommonServices common, IFileServices fileSrv, ITranslateServices translate)
        {
            _httpContextAccessor = httpContextAccessor;
            _rep = rep;
            _common = common;
            _translate = translate;
            PageLimitAdmin = common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
            DateFormat = common.GetConfigValue(ConstantStrings.KeyDateFormat);
        }

        [CustomAuthorization("Feature", "VIEW")]
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

            ViewBag.PageLimitAdmin = PageLimitAdmin;
            ViewBag.DateFormat = DateFormat;
            return View();
        }
        public IActionResult Compose(string lang)
        {
            _httpContextAccessor.HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Compose");
        }
        [CustomAuthorization("Feature", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj, IFormFile PicThumb, string imagesList, string multiLangImagesList, string listCates)
        {
            List<Temp_Images> objImages = JsonConvert.DeserializeObject<List<Temp_Images>>(imagesList);
            List<Temp_MultiLang_Images> objLangImages = JsonConvert.DeserializeObject<List<Temp_MultiLang_Images>>(multiLangImagesList);

            FeatureDetail objDetail = JsonConvert.DeserializeObject<FeatureDetail>(stringObj);
            List<MultiLang_FeatureDetail> objList = JsonConvert.DeserializeObject<List<MultiLang_FeatureDetail>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Insert(objDetail, objList, PicThumb, objImages, objLangImages, listCates), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Feature", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {

            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("Feature", "VIEW")]
        public JsonResult Edit(int Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("Feature", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj, IFormFile PicThumb, string imagesList, string imagesDeleteList, string multiLangImagesList, string listCates)
        {
            List<Temp_Images> objImages = JsonConvert.DeserializeObject<List<Temp_Images>>(imagesList);
            List<Temp_MultiLang_Images> objLangImages = JsonConvert.DeserializeObject<List<Temp_MultiLang_Images>>(multiLangImagesList);

            List<Temp_Images> listDeleteImages = JsonConvert.DeserializeObject<List<Temp_Images>>(imagesDeleteList);


            FeatureDetail objDetail = JsonConvert.DeserializeObject<FeatureDetail>(stringObj);
            List<MultiLang_FeatureDetail> objList = JsonConvert.DeserializeObject<List<MultiLang_FeatureDetail>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Update(objDetail, objList, PicThumb, listDeleteImages, objImages, objLangImages, listCates), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Feature", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("Feature", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize;

            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });

        }
        [CustomAuthorization("Feature", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Enable(Pid, Enabled), listData = _rep.LoadData(search) });

        }
        public ActionResult OpenAddModal(string lang)
        {
            lang = "vi";
            _httpContextAccessor.HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Compose");
        }
        [CustomAuthorization("Feature", "ADD")]
        public JsonResult Coppy(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Coppy(Pid), listData = _rep.LoadData(search) });

        }
        public JsonResult Preview(string stringObj, string stringMultiLangObj, IFormFile PicThumb)
        {
            var result = _rep.Preview(stringObj, stringMultiLangObj, PicThumb);
            if (result)
            {
                return Json(_translate.GetStringWithLangAdmin("preview.feature", "vi"));
            }
            return Json("");
        }
        public JsonResult MoveRow(long from, long to)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.MoveRow(from, to), list = _rep.LoadData(search) });
        }
        public JsonResult UpdateOrder(long Pid, int Order)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.UpdateOrder(Pid, Order), list = _rep.LoadData(search) });
        }
        public JsonResult SaveStatus(long pid, bool value, string type)
        {
            return Json(new { isError = _rep.SaveStatus(pid, value, type) });
        }

    }
}