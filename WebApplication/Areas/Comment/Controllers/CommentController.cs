using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CMS.Areas.Shared.Helper;
using CMS.Areas.Comment.Models;
using CMS.Services.CommonServices;
using DTO.Common;
using DTO;

namespace CMS.Areas.Comment.Controllers
{
    [Area("Comment")]
    public class CommentController : Controller
    {

        public string PageLimitAdmin = "";
        private readonly ICommentRepository _rep;
        private readonly ICommonServices _core;
        private readonly DBContext _dBContext;

        public CommentController(ICommentRepository rep, ICommonServices core)
        {
            PageLimitAdmin = core.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
            _rep = rep;
            _core = core;
        }
        [CustomAuthorization("Comment", "VIEW")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Modal(string lang)
        {
            HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Modal");
        }
        public JsonResult LoadByCate(string cate)
        {
            TempData["cate"] = cate;
            return Json("/b-admin/Services/");
        }
        #region action Detail
        [CustomAuthorization("Comment", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj, IFormFile PicThumb, IFormFile PicThumb2)
        {
            Models.Comment objDetail = JsonConvert.DeserializeObject<Models.Comment>(stringObj);
            List<MultiLang_Comment> objList = JsonConvert.DeserializeObject<List<MultiLang_Comment>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Insert(objDetail, objList, PicThumb, PicThumb2), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Comment", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {

            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("Comment", "VIEW")]
        public JsonResult Edit(long Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("Comment", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj, IFormFile PicThumb, IFormFile PicThumb2)
        {
            Models.Comment objDetail = JsonConvert.DeserializeObject<Models.Comment>(stringObj);
            List<MultiLang_Comment> objList = JsonConvert.DeserializeObject<List<MultiLang_Comment>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Update(objDetail, objList, PicThumb, PicThumb2), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Comment", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData(search) });
        }
        [CustomAuthorization("Comment", "DELETE")]
        public JsonResult DeleteAll(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);
            return Json(new { isError = _rep.DeleteAll(Pid), listData = _rep.LoadData(search) });
        }
        [CustomAuthorization("Comment", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData(search) });

        }
        [CustomAuthorization("Comment", "EDIT")]
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
        [CustomAuthorization("Comment", "ADD")]
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