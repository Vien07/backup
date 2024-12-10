using System.Linq;
using CMS.Areas.Admin.Models;
using CMS.Areas.Shared.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTO;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;

namespace CMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        public string PageLimitAdmin = "";
        private readonly IUsersRepository _rep;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;

        public UsersController(IUsersRepository rep,
                               IFileServices fileServices, ICommonServices common)
        {
            _rep = rep;
            _fileServices = fileServices;
            _common = common;

        }
        [CustomAuthorization("Users", "View")]
        public IActionResult Index()
        {
            ViewBag.DateFormat = _common.GetConfigValue(ConstantStrings.KeyDateFormat);
            return View();
        }
        public IActionResult UserEdit(int Pid)
        {
            ViewBag.Data = _rep.Edit(Pid);
            return View();
        }
        public IActionResult UserInfo()
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            ViewBag.Data = _rep.GetUserInfo();
            return View();
        }
        #region Action
        public JsonResult GetData()
        {
            var data = _rep.GetData();
            return Json(new { jsData = data });
        }
        [HttpPost]
        public JsonResult Insert(User data, IFormFile Images)
        {
            if (!ModelState.IsValid)
            {

                string errors = string.Join("; ", ModelState.Values
                      .SelectMany(x => x.Errors)
                      .Select(x => x.ErrorMessage));
                return Json(new { error = false, messError = errors });
            }
            dynamic valid = _rep.Validation(data);
            if (valid.error)
            {
                return Json(new { error = false, messError = valid.errorMess });
            }

            if (Images != null)
            {
                dynamic kt = _fileServices.SaveFile(Images, ConstantStrings.UrlUserImages, data.Code);
                if (!kt.isError)
                {
                    _fileServices.ResizeThumbImage(Images, ConstantStrings.UrlUserImages, kt.fileName);
                    data.Avatar = kt.fileName;
                }
            }
            else
            {
                data.Avatar = "default-avatar.png";
            }

            var rs = _rep.Insert(data);
            var jsData = _rep.GetData();
            return Json(new { Error = rs, jsData = jsData });
        }
        [HttpPost]
        public IActionResult Update(User data, IFormFile Images, string newPassWord, int Type)
        {
            var code = HttpContext.Session.GetString("UserCode");


            var rs = _rep.Update(data, Images, newPassWord, Type);
            if (data.Code == code)
            {
                return Json(new { error = "reset" });

            }
            var jsData = _rep.GetData();
            return Json(new { error = rs, jsData = jsData });
        }
        public JsonResult Edit(int code)
        {
            var data = _rep.Edit(code);
            return Json(new { jsData = data });
        }
        public JsonResult Enable(string code, bool active)
        {
            var data = _rep.Enable(code, active);
            return Json(new { Error = data });
        }
        public JsonResult Delete(string code)
        {

            var rs = _rep.Delete(code);
            var jsData = _rep.GetData();
            return Json(new { Error = rs, jsData = jsData });
        }
        public JsonResult Search(SearchDto searchString)
        {
            var data = _rep.Search(searchString);
            return Json(new { jsData = data });
        }
        #endregion
    }
}