using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Areas.Shared.Helper;
using Microsoft.AspNetCore.Mvc;
using CMS.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using CMS.Services.FileServices;
using CMS.Services.CommonServices;
using DTO.Common;

namespace CMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GroupController : Controller
    {
        private readonly IGroupRepository _rep;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;

        public GroupController(IGroupRepository rep,
                               IFileServices fileServices, ICommonServices common)
        {
            _rep = rep;
            _fileServices = fileServices;
            _common = common;

        }
        [CustomAuthorization("Group", "View")]
        public IActionResult Index()
        {
            return View();
        }
        #region Action

        public JsonResult GetData()
        {
            var data = _rep.GetData();
            return Json(new { jsData = data });
        }
        public JsonResult Insert(GroupUser data)
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
                return Json(new { error = false, messError = valid.messError });
            }


            var rs = _rep.Insert(data);
            var jsData = _rep.GetData();
            return Json(new { Error = rs, jsData = jsData });
        }
        public JsonResult Update(GroupUser data)
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
                return Json(new { error = false, messError = valid.messError });
            }

            var rs = _rep.Update(data);
            var jsData = _rep.GetData();
            return Json(new { error = rs, jsData = jsData });
        }
        public JsonResult Edit(int code)
        {
            var data = _rep.Edit(code);
            return Json(new { jsData = data });
        }
        public JsonResult Enable(int code, bool active)
        {
            var data = _rep.Enable(code, active);
            return Json(new { Error = data });
        }
        public JsonResult Delete(int code)
        {
            if (!_rep.Count(code))
            {
                return Json(new { error = false, messError = "Có user trong nhóm!" });
            }
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