
using Admin.Authorization.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using PartialViews = Admin.Authorization.Constants.StaticPath.PartialView;

namespace Admin.Authorization.Controllers
{
    public class AccountEditPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Quản lý tài khoản", "Chi tiết", "", "/User");

        public UserDetail UserModel = new UserDetail();
        public List<Admin.Authorization.Database.GroupRole> ListGroupRole;
        public List<long> ListGroupPermission;
    }
    public partial class AccountController
    {
        [HttpGet("Account/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            AccountEditPageModel _editModel = new AccountEditPageModel();
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.UserModel = data;
                }
            }
            Authorization.Models.ParamSearch paramSearch = new Authorization.Models.ParamSearch();
            _editModel.ListGroupRole = _srvGroupRole.GetList(paramSearch).Data.ToList();
            _editModel.ListGroupPermission = _srv.GetLisPermissionGroup(id).Data;

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(AccountModelEdit input)
        {
            input.CreateUser = CreateUser;
            input.UpdateUser = CreateUser;
            ParamSearch search = new ParamSearch();

            var res = _srv.Save(input);

            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _srv.GetList(search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }

        [HttpPost]
        public JsonResult ResetPassword(int pid)
        {
            var res = _srv.ResetPassword(pid);
            return Json(res);
        }
    }
}
