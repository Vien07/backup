using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Areas.Shared.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Areas.Contact.Controllers
{
    [Area("Contact")]
    public class ContactController : Controller
    {
        private readonly IContactInfoRepository _rep;
        private readonly DBContext _dBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ContactController(DBContext dBContext, IHttpContextAccessor httpContextAccessor, IContactInfoRepository rep)
        {
            _rep = rep;
            _httpContextAccessor = httpContextAccessor;
            _dBContext = dBContext;
        }
        [CustomAuthorization("Contact", "VIEW")]
        public IActionResult Index()
        {
            return View();
        }
        #region Action
        [HttpPost]
        [CustomAuthorization("Contact", "EDIT")]
        public JsonResult Update(IFormCollection formData, string langKey)
        {
            dynamic kt = _rep.Update(formData, langKey);
            return Json(new { jsData = "", keyError = kt });
        }
        [HttpGet]
        [CustomAuthorization("Contact", "VIEW")]
        public JsonResult GetData(string langKey)
        {
            var data = _rep.GetData(langKey);
            return Json(new { jsData = data });
        }
        #endregion
    }
}