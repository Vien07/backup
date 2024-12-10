using CMS.Areas.Shared.Helper;
using CMS.Services.CommonServices;
using DTO;
using DTO.Common;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Areas.Contact.Controllers
{
    [Area("Contact")]
    public class EnquireListController : Controller
    {
        private readonly IEnquireListRepository _rep;
        private readonly ICommonServices _common;
        private string DateFormat = "";

        public EnquireListController(IEnquireListRepository rep, ICommonServices common)
        {
            _rep = rep;
            _common = common;
            DateFormat = _common.GetConfigValue(ConstantStrings.KeyDateFormat);
        }
        [CustomAuthorization("EnquireList", "VIEW")]
        public IActionResult Index()
        {
            ViewBag.DateFormat = DateFormat;
            return View();
        }
        #region Action
        [CustomAuthorization("EnquireList", "VIEW")]
        public JsonResult GetData(SearchDto search)
        {
            return Json(_rep.GetData(search));
        }
        [CustomAuthorization("EnquireList", "VIEW")]
        public JsonResult ReadContact(long Pid)
        {
            return Json(_rep.ReadContact(Pid));
        }
        [CustomAuthorization("EnquireList", "DELETE")]
        public JsonResult Delete(long[] Pid, SearchDto search)
        {
            return Json(new { error = _rep.Delete(Pid), listData = _rep.GetData(search) });

        }
        [CustomAuthorization("EnquireList", "VIEW")]
        public JsonResult Seen(long[] Pid, SearchDto search)
        {
            return Json(new { error = _rep.Seen(Pid), listData = _rep.GetData(search) });

        }
        [CustomAuthorization("EnquireList", "VIEW")]
        public JsonResult NotSeen(long[] Pid, SearchDto search)
        {
            return Json(new { error = _rep.NotSeen(Pid), listData = _rep.GetData(search) });

        }
        #endregion
    }
}