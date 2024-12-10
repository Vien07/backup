using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.Areas.Shared.Helper;
using CMS.Services.CommonServices;
using CMS.Services.FileServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PermitController : Controller
    {
        private readonly IPermitRepository _rep;
        private readonly ICommonServices _common;
        private readonly IFileServices _fileServices;

        public PermitController(IPermitRepository rep,
                               IFileServices fileServices, ICommonServices common)
        {
            _rep = rep;
            _fileServices = fileServices;
            _common = common;
        }
        [CustomAuthorization("Permit", "VIEW")]
        public IActionResult GroupIndex(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.GroupUserCode = id;
                return View();
            }
            else
            {
                return Redirect("~/b-admin/Group/Index");
            }
        }
        [CustomAuthorization("Permit", "VIEW")]
        public IActionResult UserIndex(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.GroupUserCode = id;
                return View();
            }
            else
            {
                return Redirect("~/b-admin/Users/Index");
            }
        }

        public string GetDataGroup(int ? groupUserCode, string txtSearch)
        {
            if(groupUserCode != null)
            {
                var model = _rep.GetDataGroup(groupUserCode, txtSearch);
                return model;
            }
            else
            {
                return "Error";
            }
        }
        public string GetDataUser(int? userCode, string txtSearch)
        {
            if (userCode != null)
            {
                var model = _rep.GetDataUser(userCode, txtSearch);
                return model;
            }
            else
            {
                return "Error";
            }
        }
        public string InsertGroupPermission(int ? groupUserCode, string id, bool isChecked)
        {
            if (groupUserCode != null)
            {
                var mess = _rep.InsertGroupPermission(Convert.ToInt16(groupUserCode), id, isChecked);
                return mess;
            }
            return "Error";
        }
        public string InsertGroupPermissionUser(int? userCode, string id, bool isChecked)
        {
            if (userCode != null)
            {
                var mess = _rep.InsertGroupPermissionUser(Convert.ToInt16(userCode), id, isChecked);
                return mess;
            }
            return "Error";
        }
    }
}