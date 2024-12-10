using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CMS.Areas.Shared.Helper;
using CMS.Areas.Customer.Models;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;
using CMS.Services.TranslateServices;

namespace CMS.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _rep;
        private readonly IFileServices _fileSrv;
        private readonly ITranslateServices _translate;
        private readonly ICommonServices _common;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string PageLimitAdmin = "";
        private string DateFormat = "";
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;

        public CustomerController(IHttpContextAccessor httpContextAccessor, ICustomerRepository rep,
                            ICommonServices common, IFileServices fileSrv, ITranslateServices translate)
        {
            _httpContextAccessor = httpContextAccessor;
            _rep = rep;
            _common = common;
            _translate = translate;
            _fileSrv = fileSrv;
            PageLimitAdmin = common.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
            DateFormat = common.GetConfigValue(ConstantStrings.KeyDateFormat);
        }

        [CustomAuthorization("Customer", "VIEW")]
        public IActionResult Index()
        {
            ViewBag.PageLimitAdmin = PageLimitAdmin;
            ViewBag.DateFormat = DateFormat;
            return View();
        }
        [CustomAuthorization("Customer", "VIEW")]
        public IActionResult CreateOrUpdate(string pid)
        {
            ViewBag.Pid = pid;
            ViewBag.DateFormat = DateFormat;
            //ViewBag.Customer = _rep.LoadCustomer(Convert.ToInt32(pid));
            //ViewBag.OrderList = _rep.LoadOrders(Convert.ToInt32(pid));
            return View();
        }
        [CustomAuthorization("Customer", "VIEW")]
        public JsonResult LoadData(SearchDto search)
        {
            ViewBag.DateFormat = DateFormat;
            var data = _rep.LoadData(search);
            return Json(data);
        }
        [CustomAuthorization("Customer", "ADD")]
        public JsonResult Insert(string customer, IFormFile avatar)
        {
            var customerObj = JsonConvert.DeserializeObject<Models.Customer>(customer);
            return Json(new { error = _rep.Insert(customerObj, avatar) });
        }

        [CustomAuthorization("Customer", "EDIT")]
        public JsonResult Update(string customer, IFormFile avatar)
        {
            var customerObj = JsonConvert.DeserializeObject<Models.Customer>(customer);
            return Json(new { error = _rep.Update(customerObj, avatar) });
        }

        [CustomAuthorization("Customer", "EDIT")]
        public JsonResult Edit(int pid)
        {
            return Json(_rep.Edit(pid));
        }
        public JsonResult LoadOrders(int pid)
        {
            return Json(_rep.LoadOrders(pid));
        }

        [CustomAuthorization("Customer", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; 
            var result = _rep.Delete(Pid);
            return Json(new { isError = result.isError, messError = result.messError, jsData = _rep.LoadData(search) });
        }
        [CustomAuthorization("Customer", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize;
            var result = _rep.Delete(Pid);
            return Json(new { isError = result.isError, messError = result.messError, jsData = _rep.LoadData(search) });

        }
        [CustomAuthorization("Customer", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto search = new SearchDto();
            search.Page = 1;
            search.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Enable(Pid, Enabled), listData = _rep.LoadData(search) });

        }
    }
}