using CMS.Areas.Shared.Helper;
using CMS.Services.CommonServices;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMS.Areas.Translation.Controllers
{
    [Area("Translation")]
    public class TranslationController : Controller
    {
        public string PageLimitAdmin = "";
        private readonly ITranslationRepository _rep;
        private readonly ICommonServices _core;
        private readonly DBContext _dBContext;

        public TranslationController(DBContext dBContext, ITranslationRepository rep, ICommonServices core)
        {
            PageLimitAdmin = core.GetConfigValue(ConstantStrings.KeyPageLimitAdmin);
            _dBContext = dBContext;
            _rep = rep;
            _core = core;
        }
        [CustomAuthorization("Translation", "VIEW")]
        public IActionResult Index()
        {

            //Startup._httpContextAccessor.HttpContext.Session.SetString("LangCompose", "");
            //var a =Startup._httpContextAccessor.HttpContext.Session.GetString("UserAvatar");
            return View();
        }
        #region action Detail
        [CustomAuthorization("Translation", "VIEW")]
        public JsonResult LoadData()
        {

            var data = _rep.LoadData();
            return Json(data);
        }
        [CustomAuthorization("Translation", "EDIT")]
        public JsonResult Update(string data)
        {
            return Json(new { error = _rep.Update(data) });
        }
        #endregion

    }
}