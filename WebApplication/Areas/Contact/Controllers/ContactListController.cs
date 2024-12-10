using Microsoft.AspNetCore.Mvc;
using CMS.Areas.Shared.Helper;
using Microsoft.AspNetCore.Http;
using DTO;
using CMS.Services.CommonServices;
using DTO.Common;

namespace CMS.Areas.Contact.Controllers
{
    [Area("Contact")]
    public class ContactListController : Controller
    {
        private readonly IContactListRepository _rep;
        private readonly ICommonServices _common;
        private string DateFormat = "";

        public ContactListController(IContactListRepository rep, ICommonServices common)
        {
            _rep = rep;
            _common = common;
            DateFormat = _common.GetConfigValue(ConstantStrings.KeyDateFormat);
        }
        [CustomAuthorization("ContactList", "VIEW")]
        public IActionResult Index()
        {
            ViewBag.DateFormat = DateFormat;
            return View();
        }
        #region Action
        [CustomAuthorization("ContactList", "VIEW")]
        public JsonResult GetData(SearchDto search)
        {           
            return Json(_rep.GetData(search));
        }
        [CustomAuthorization("ContactList", "VIEW")]
        public JsonResult ReadContact(long Pid)
        {
            return Json(_rep.ReadContact(Pid));
        }
        [CustomAuthorization("ContactList", "DELETE")]
        public JsonResult Delete(long[] Pid,SearchDto search)
        {
            return Json(new { error = _rep.Delete(Pid), listData = _rep.GetData(search) });

        }
        [CustomAuthorization("ContactList", "VIEW")]
        public JsonResult Seen(long[] Pid, SearchDto search)
        {
            return Json(new { error = _rep.Seen(Pid), listData = _rep.GetData(search) });

        }
        [CustomAuthorization("ContactList", "VIEW")]
        public JsonResult NotSeen(long[] Pid, SearchDto search)
        {
            return Json(new { error = _rep.NotSeen(Pid), listData = _rep.GetData(search) });

        }
        #endregion
    }
}