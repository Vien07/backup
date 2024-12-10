using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CMS.Areas.Shared.Helper;
using CMS.Areas.Promotion.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using CMS.Services.TranslateServices;
using DTO.Product;

namespace CMS.Areas.Promotion.Controllers
{
    [Area("Promotion")]
    public class PromotionController : Controller
    {
        private readonly IPromotionRepository _rep;
        private readonly ITranslateServices _translate;
        private readonly ICommonServices _common;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string PageLimitAdmin = "";
        private string DateFormat = "";
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;

        public PromotionController(IHttpContextAccessor httpContextAccessor, IPromotionRepository rep,
                            ICommonServices common, IFileServices fileSrv, ITranslateServices translate)
        {
            _httpContextAccessor = httpContextAccessor;
            _rep = rep;
            _common = common;
            _translate = translate;
            PageLimitAdmin = common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
            DateFormat = common.GetConfigValue(ConstantStrings.KeyDateFormat);
        }

        [CustomAuthorization("Promotion", "VIEW")]
        public IActionResult Index()
        {
            ViewBag.PageLimitAdmin = PageLimitAdmin;
            ViewBag.DateFormat = DateFormat;
            return View();
        }
        [CustomAuthorization("Promotion", "VIEW")]
        public IActionResult Product(int id)
        {
            ViewBag.PageLimitAdmin = PageLimitAdmin;
            ViewBag.DateFormat = DateFormat;
            ViewBag.Id = id;
            //ViewBag.Products = _rep.GetAllProduct();
            return View();
        }
        public IActionResult Compose(string lang)
        {
            _httpContextAccessor.HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("Compose");
        }
        [CustomAuthorization("Promotion", "ADD")]
        public JsonResult Insert(string stringObj, string stringMultiLangObj, IFormFile PicThumb, string imagesList, string multiLangImagesList, string listCates)
        {
            List<Temp_Images> objImages = JsonConvert.DeserializeObject<List<Temp_Images>>(imagesList);
            List<Temp_MultiLang_Images> objLangImages = JsonConvert.DeserializeObject<List<Temp_MultiLang_Images>>(multiLangImagesList);

            PromotionDetail objDetail = JsonConvert.DeserializeObject<PromotionDetail>(stringObj);
            List<MultiLang_PromotionDetail> objList = JsonConvert.DeserializeObject<List<MultiLang_PromotionDetail>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Insert(objDetail, objList, PicThumb, objImages, objLangImages, listCates), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Promotion", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {

            var data = _rep.LoadData(search);
            return Json(data);
        }
        public JsonResult LoadProducts(SearchDto search)
        {

            var data = _rep.LoadProducts(search);
            return Json(data);
        }
        [CustomAuthorization("Promotion", "VIEW")]
        public JsonResult Edit(int Pid)
        {

            var data = _rep.Edit(Pid);
            return Json(data);
        }
        [CustomAuthorization("Promotion", "EDIT")]
        public JsonResult Update(string stringObj, string stringMultiLangObj, IFormFile PicThumb, string imagesList, string imagesDeleteList, string multiLangImagesList, string listCates)
        {
            List<Temp_Images> objImages = JsonConvert.DeserializeObject<List<Temp_Images>>(imagesList);
            List<Temp_MultiLang_Images> objLangImages = JsonConvert.DeserializeObject<List<Temp_MultiLang_Images>>(multiLangImagesList);

            List<Temp_Images> listDeleteImages = JsonConvert.DeserializeObject<List<Temp_Images>>(imagesDeleteList);


            PromotionDetail objDetail = JsonConvert.DeserializeObject<PromotionDetail>(stringObj);
            List<MultiLang_PromotionDetail> objList = JsonConvert.DeserializeObject<List<MultiLang_PromotionDetail>>(stringMultiLangObj);

            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = Convert.ToInt32(PageLimitAdmin);

            return Json(new { error = _rep.Update(objDetail, objList, PicThumb, listDeleteImages, objImages, objLangImages, listCates), list = _rep.LoadData(search) });
        }
        [CustomAuthorization("Promotion", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("Promotion", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize;

            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(search) });

        }
        [CustomAuthorization("Promotion", "EDIT")]
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
        [CustomAuthorization("Promotion", "ADD")]
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
                return Json(_translate.GetStringWithLangAdmin("preview.news", "vi"));
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

        public JsonResult GetAllPromoProduct(string listPid)
        {
            var list = JsonConvert.DeserializeObject<List<long>>(listPid);
            return Json(_rep.GetAllPromoProduct(list));
        }
        public JsonResult GetPromo(long pid)
        {
            return Json(_rep.GetPromo(pid));
        }
        public JsonResult UpdatePromotionProduct(string data, long pid)
        {
            var list = JsonConvert.DeserializeObject<List<PromoProduct>>(data);
            return Json(_rep.UpdatePromotionProduct(list, pid));
        }
        public JsonResult CheckValid(string listPid, long pid)
        {
            var list = JsonConvert.DeserializeObject<List<long>>(listPid);
            return Json(_rep.CheckValid(list, pid));
        }
    }
}