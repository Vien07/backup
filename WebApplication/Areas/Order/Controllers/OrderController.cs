using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CMS.Areas.Shared.Helper;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO;
using DTO.Common;
using CMS.Services.TranslateServices;
using static DTO.Cart.EnumOrder;
using CMS.Areas.Shared.Controllers;
using DTO.Customer;
using DTO.Cart;
using Pchp.Core.Dynamic.RuntimeChain;
using System.Threading.Tasks;

namespace CMS.Areas.Order.Controllers
{
    [Area("Order")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _rep;
        private readonly IFileServices _fileSrv;
        private readonly ITranslateServices _translate;
        private readonly ICommonServices _common;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string PageLimitAdmin = "";
        private string DateFormat = "";
        private int DefaultPageSize = ConstantStrings.DefaultPageSize;

        public OrderController(IHttpContextAccessor httpContextAccessor, IOrderRepository rep,
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

        [CustomAuthorization("Order", "VIEW")]
        public IActionResult Index()
        {
            ViewBag.PageLimitAdmin = PageLimitAdmin;
            ViewBag.DateFormat = DateFormat;
            return View();
        }
        [CustomAuthorization("Order", "VIEW")]
        public IActionResult CreateOrUpdate(string pid)
        {
            ViewBag.DateFormat = DateFormat;
            ViewBag.Pid = pid;
            return View();
        }
        [CustomAuthorization("Order", "VIEW")]
        public IActionResult CreateOrUpdate2(string pid)
        {
            ViewBag.DateFormat = DateFormat;
            ViewBag.Pid = pid;
            return View();
        }
        [CustomAuthorization("Order", "VIEW")]
        public IActionResult CustomerOrderList(string pid)
        {
            ViewBag.Pid = pid;
            return View();
        }


        [CustomAuthorization("Order", "VIEW")]
        public JsonResult LoadData(SearchDto SearchDto)
        {

            var data = _rep.LoadData(SearchDto);
            return Json(data);
        }
        [CustomAuthorization("Order", "ADD")]
        public JsonResult Insert(OrderInformation order)
        {
            return Json(new { error = _rep.Insert(order) });
        }

        [CustomAuthorization("Order", "EDIT")]
        public JsonResult Update(OrderInformation order)
        {

            return Json(new { error = _rep.Update(order) });
        }

        [CustomAuthorization("Order", "EDIT")]
        public JsonResult Edit(int pid)
        {
            return Json(_rep.Edit(pid));
        }

        [CustomAuthorization("Order", "DELETE")]
        public JsonResult Delete(int Pid)
        {
            SearchDto SearchDto = new SearchDto();
            SearchDto.Page = 1;
            SearchDto.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(SearchDto) });
        }
        [CustomAuthorization("Order", "DELETE")]
        public JsonResult DeleteMulti(long[] Pid)
        {
            SearchDto SearchDto = new SearchDto();
            SearchDto.Page = 1;
            SearchDto.PageNumber = DefaultPageSize;

            return Json(new { isError = _rep.Delete(Pid), jsData = _rep.LoadData(SearchDto) });

        }
        [CustomAuthorization("Order", "EDIT")]
        public JsonResult Enable(long[] Pid, bool Enabled)
        {
            SearchDto SearchDto = new SearchDto();
            SearchDto.Page = 1;
            SearchDto.PageNumber = DefaultPageSize; ;
            return Json(new { isError = _rep.Enable(Pid, Enabled), listData = _rep.LoadData(SearchDto) });
        }
        public JsonResult LoadProductList(string key, int catePid)
        {
            return Json(_rep.LoadProductList(key, catePid));
        }
        public JsonResult LoadProductDetail(long productId)
        {
            return Json(_rep.LoadProductDetail(productId));
        }
        public JsonResult LoadOrderTable(string orderString, decimal shipFee, decimal deposit)
        {
            return Json(_rep.LoadOrderTable(orderString, shipFee, deposit));
        }

        [CustomAuthorization("Order", "EDIT")]
        public JsonResult ChangeStatus(long[] Pid, int value)
        {
            return Json(new { isError = _rep.ChangeStatus(Pid, value) });
        }
        [CustomAuthorization("Order", "EDIT")]
        public JsonResult ChangePaymentMethod(long[] Pid, int value)
        {
            return Json(new { isError = _rep.ChangePaymentMethod(Pid, value) });
        }
        [CustomAuthorization("Order", "EDIT")]
        public JsonResult ChangeIsPayment(long[] Pid, bool value)
        {
            return Json(new { isError = _rep.ChangeIsPayment(Pid, value) });
        }
        [CustomAuthorization("Order", "EDIT")]
        public async Task<JsonResult> SendMail(long[] Pid)
        {
            return Json(new { isError = await _rep.SendMail(Pid) });
        }

        [CustomAuthorization("Order", "EDIT")]
        public async Task<JsonResult> ExportVAT(long[] Pid, bool isSendMail)
        {
            return Json(new { isError = await _rep.ExportVAT(Pid, isSendMail) });
        }
        [CustomAuthorization("Order", "VIEW")]
        public JsonResult GetInfoCustomer(string email)
        {
            return Json(_rep.GetInfoCustomer(email));
        }
        [CustomAuthorization("Order", "VIEW")]
        public JsonResult GetPrice(long ProductPid, long ProductCatePid)
        {
            return Json(_rep.GetPrice(ProductPid, ProductCatePid));
        }
        [CustomAuthorization("Order", "ADD")]
        public ActionResult OpenAddModal(string lang)
        {
            _httpContextAccessor.HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("OrderDetailModal");
        }
        [CustomAuthorization("Order", "VIEW")]
        public IActionResult OrderDetailModal(string lang)
        {
            HttpContext.Session.SetString("LangCompose", lang);
            return PartialView("OrderDetailModal");
        }
        [CustomAuthorization("Order", "EDIT")]
        public JsonResult InsertCard(CardInformation card)
        {
            return Json(new { error = _rep.InsertCard(card) });
        }
        [CustomAuthorization("Order", "EDIT")]
        public JsonResult UpdateCard(CardInformation card)
        {
            return Json(new { error = _rep.UpdateCard(card) });
        }
        [CustomAuthorization("Order", "VIEW")]
        public JsonResult LoadDataCard(SearchDto SearchDto)
        {

            var data = _rep.LoadDataCard(SearchDto);
            return Json(data);
        }
        [CustomAuthorization("Order", "EDIT")]
        public JsonResult EditCard(int pid)
        {
            return Json(_rep.EditCard(pid));
        }
        [CustomAuthorization("Order", "DELETE")]
        public JsonResult DeleteCard(int Pid, string orderPid)
        {
            SearchDto SearchDto = new SearchDto();
            SearchDto.Page = 1;
            SearchDto.PageNumber = DefaultPageSize;
            SearchDto.OrderPid = orderPid;
            return Json(new { isError = _rep.DeleteCard(Pid), jsData = _rep.LoadDataCard(SearchDto) });
        }
        [CustomAuthorization("Order", "DELETE")]
        public JsonResult DeleteMultiCard(long[] Pid, string orderPid)
        {
            SearchDto SearchDto = new SearchDto();
            SearchDto.Page = 1;
            SearchDto.PageNumber = DefaultPageSize;
            SearchDto.OrderPid = orderPid;
            return Json(new { isError = _rep.DeleteCard(Pid), jsData = _rep.LoadDataCard(SearchDto) });

        }
        [CustomAuthorization("Order", "EDIT")]
        public JsonResult UploadCSV(IFormFile File, string OrderPid)
        {
            var result = _rep.UploadCSV(File, OrderPid);
            return Json(new { isError = result.isError, data = result.data, messError = result.messError});;

        }
        [CustomAuthorization("Order", "VIEW")]
        public JsonResult ExportCSV(string OrderPid)
        {
            var result = _rep.ExportCSV(OrderPid);
            return Json(new { isError = result.isError, data = result.data, messError = result.messError }); ;

        }
        public JsonResult ApplyDiscountCode(decimal price, string discountCode)
        {
            var result = _rep.ApplyDiscountCode(price, discountCode);
            return Json(new { isError = result.isError, data = result.data, messError = result.messError }); ;

        }
    }
}