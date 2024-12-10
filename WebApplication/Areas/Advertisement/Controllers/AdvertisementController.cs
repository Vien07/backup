using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CMS.Areas.Shared.Helper;
using CMS.Areas.Advertisement.Models;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO;
using DTO.Common;

namespace CMS.Areas.Advertisement.Controllers
{
    [Area("Advertisement")]
    public class AdvertisementController : Controller
    {
        public string PageLimitAdmin = "";

        private readonly IAdvertisementRepository _rep;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;
        private readonly DBContext _dBContext;
        private string KeyPageLimitAdmin = ConstantStrings.KeyPageLimitAdmin;
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;

        public AdvertisementController(DBContext dBContext, IAdvertisementRepository rep,
                               IFileServices fileServices, ICommonServices common)
        {
            _dBContext = dBContext;
            _rep = rep;
            _fileServices = fileServices;
            _common = common;
            PageLimitAdmin = _common.GetConfigValue(KeyPageLimitAdmin);
        }
        [CustomAuthorization("Advertisement", "VIEW")]
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
        [CustomAuthorization("Advertisement", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj, IFormFile PicThumb, string listCates)
        {
            Models.Advertisement objDetail = JsonConvert.DeserializeObject<Models.Advertisement>(stringObj);
            List<MultiLang_Advertisement> objList = JsonConvert.DeserializeObject<List<MultiLang_Advertisement>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;

            return Json(new { error = _rep.Insert(objDetail, objList, PicThumb, listCates), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Advertisement", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {

            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("Advertisement", "VIEW")]
        public JsonResult Edit(long Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("Advertisement", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj, IFormFile PicThumb, string listCates)
        {
            Models.Advertisement objDetail = JsonConvert.DeserializeObject<Models.Advertisement>(stringObj);
            List<MultiLang_Advertisement> objList = JsonConvert.DeserializeObject<List<MultiLang_Advertisement>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;

            return Json(new { error = _rep.Update(objDetail, objList, PicThumb, listCates), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Advertisement", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData(search) });
        }
        [CustomAuthorization("Advertisement", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize;

            return Json(new { isError = _rep.Delete(Pid), listData = _rep.LoadData(search) });

        }
        [CustomAuthorization("Advertisement", "EDIT")]
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