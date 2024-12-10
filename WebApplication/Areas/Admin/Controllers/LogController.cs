using CMS.Areas.Shared.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;

namespace CMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LogController : Controller
    {
        private readonly ILogRepository _rep;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;

        public LogController(ILogRepository rep,
                               IFileServices fileServices, ICommonServices common)
        {

            _rep = rep;
            _fileServices = fileServices;
            _common = common;
        }
        [CustomAuthorization("Log", "View")]
        public IActionResult Index()
        {
            ViewBag.DateFormat = _common.GetConfigValue(ConstantStrings.KeyDateFormat);

            return View();
        }

        [CustomAuthorization("Log", "View")]
        public IActionResult Error()
        {
            ViewBag.DateFormat = _common.GetConfigValue(ConstantStrings.KeyDateFormat);

            return View();
        }
        #region Action
        public JsonResult GetList(SearchDto search)
        {
            var data = _rep.GetList(search);
            return Json(data);
        }
        public JsonResult GetListError(SearchDto search)
        {
            var data = _rep.GetListError(search);
            return Json(data);
        }
        public ActionResult LinkPost(string pidCate, string pidDetail)
        {
            //Không còn link từ log nữa nên tắt
            //if (pidCate == ConstantStrings.NewsId.ToString())
            //{
            //    return Redirect("/b-admin/News?editPid=" + pidDetail);
            //}          
            //else if (pidCate == ConstantStrings.BookId.ToString())
            //{
            //    return Redirect("/b-admin/Book?editPid=" + pidDetail);
            //}  
            //else if (pidCate == ConstantStrings.AboutId.ToString())
            //{
            //    return Redirect("/b-admin/About?editPid=" + pidDetail);
            //}
            return View();
        }
        #endregion
    }
}