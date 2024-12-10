using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CMS.Areas.Shared.Helper;
using CMS.Areas.Recruitment.Models;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using CmsModels;
using DTO;
using DTO.Common;

namespace CMS.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class RecruitmentController : Controller
    {
        private readonly IRecruitmentRepository _rep;
        IFileServices _fileSrv;
        ICommonServices _common;
        IHttpContextAccessor _httpContextAccessor;
        // private readonly CoreServices _core;
        public string PageLimitAdmin = "";
        public string DateFormat = "";

        public RecruitmentController(DBContext _dBContext, IHttpContextAccessor httpContextAccessor, IRecruitmentRepository rep,
                             IFileServices fileSrv, ICommonServices common)
        {

            _httpContextAccessor = httpContextAccessor;
            _rep = rep;
            
            _fileSrv = fileSrv;
            _common = common;
            PageLimitAdmin = _common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
            DateFormat = _common.GetConfigValue(ConstantStrings.KeyDateFormat);
        }

        [CustomAuthorization("Recruitment", "VIEW")]
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
            //Startup._httpContextAccessor.HttpContext.Session.SetString("LangCompose", "");
            //var a =Startup._httpContextAccessor.HttpContext.Session.GetString("UserAvatar");
            return View();
        }
        public IActionResult Compose(string lang)
        {
            _httpContextAccessor.HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Compose");
        }
        [CustomAuthorization("Recruitment", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj, IFormFile PicThumb, string imagesList, string multiLangImagesList, string listCates)
        {
            List<Temp_Images> objImages = JsonConvert.DeserializeObject<List<Temp_Images>>(imagesList);
            List<Temp_MultiLang_Images> objLangImages = JsonConvert.DeserializeObject<List<Temp_MultiLang_Images>>(multiLangImagesList);

            RecruitmentDetail objDetail = JsonConvert.DeserializeObject<RecruitmentDetail>(stringObj);
            List<MultiLang_RecruitmentDetail> objList = JsonConvert.DeserializeObject<List<MultiLang_RecruitmentDetail>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Insert(objDetail, objList, PicThumb, objImages, objLangImages, listCates), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Recruitment", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {

            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("Recruitment", "VIEW")]
        public JsonResult Edit(int Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("Recruitment", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj, IFormFile PicThumb, string imagesList, string imagesDeleteList, string multiLangImagesList, string listCates)
        {
            List<Temp_Images> objImages = JsonConvert.DeserializeObject<List<Temp_Images>>(imagesList);
            List<Temp_MultiLang_Images> objLangImages = JsonConvert.DeserializeObject<List<Temp_MultiLang_Images>>(multiLangImagesList);

            List<Temp_Images> listDeleteImages = JsonConvert.DeserializeObject<List<Temp_Images>>(imagesDeleteList);


            RecruitmentDetail objDetail = JsonConvert.DeserializeObject<RecruitmentDetail>(stringObj);
            List<MultiLang_RecruitmentDetail> objList = JsonConvert.DeserializeObject<List<MultiLang_RecruitmentDetail>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Update(objDetail, objList, PicThumb, listDeleteImages, objImages, objLangImages, listCates), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Recruitment", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize; ;
            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("Recruitment", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize;

            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });

        }
        [CustomAuthorization("Recruitment", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize; ;
            return Json(new { isError = _rep.Enable(Pid, Enabled), listData = _rep.LoadData(search) });

        }
        public ActionResult OpenAddModal(string lang)
        {
            lang = "vi";
            _httpContextAccessor.HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Compose");
        }
        [CustomAuthorization("Recruitment", "ADD")]
        public JsonResult Coppy(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = ConstantStrings.DefaultPageSize; ;
            return Json(new { isError = _rep.Coppy(Pid), listData = _rep.LoadData(search) });

        }
        public JsonResult Preview(string stringObj, string stringMultiLangObj, IFormFile PicThumb, string lang, string state)
        {
            RecruitmentDetail objDetail = JsonConvert.DeserializeObject<RecruitmentDetail>(stringObj);
            List<MultiLang_RecruitmentDetail> objList = JsonConvert.DeserializeObject<List<MultiLang_RecruitmentDetail>>(stringMultiLangObj);
            TempData["detailRecruitment"] = stringObj;
            TempData["listRecruitment"] = stringMultiLangObj;
            if (PicThumb != null)
            {
                TempData["imagesBase64"] = _fileSrv.ConvertIformfileToBase64(PicThumb);
            }
            else
            {
                TempData["imagesBase64"] = objDetail.PicThumb;
            }
            TempData["lang"] = lang;
            TempData["stateRecruitment"] = state;

            string slug = _common.EncodeTitle(objList.Where(p => p.LangKey == lang).FirstOrDefault().Title);

            string url = ("url.preview-news", lang) + slug + ".html";


            return Json(url);

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
            search.PageNumber = ConstantStrings.DefaultPageSize;
            return Json(new { isError = _rep.UpdateOrder(Pid, Order), list = _rep.LoadData(search) });
        }
    }
}