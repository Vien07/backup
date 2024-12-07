using Admin.Authorization.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using PartialViews = Admin.Authorization.Constants.StaticPath.PartialViewGroupRole;

namespace Admin.Authorization.Controllers
{
    public class GroupRoleEditPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Quản lý nhóm quyền", "Chi tiết", "", "/GroupRole");

        public List<Database.ModuleRole> List;
        public List<long> ListPermission;
        public GroupRoleDetail GroupRoleModel = new GroupRoleDetail();
        public ParamSearch search;
        public PaginationModel Pagination = new PaginationModel();
    }
    public partial class GroupRoleController
    {
        [HttpGet("GroupRole/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            GroupRoleEditPageModel _editModel = new GroupRoleEditPageModel();
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.GroupRoleModel = data;
                }
            }

            ParamSearch param = new ParamSearch();
            _editModel.List = _srvModuleRole.GetListNoPagelListNotContainAnonymousRole(param).Data;
            _editModel.ListPermission = _srv.GetListPermission(id).Data;
            _editModel.Pagination = new PaginationModel();
            _editModel.Pagination.PageIndex = param.PageIndex;
            _editModel.Pagination.PageCount = _editModel.List.Count;

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(Admin.Authorization.Database.GroupRole data, long[] pidRoleSelected)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            ParamSearch search = new ParamSearch();

            var res = _srv.Save(data, pidRoleSelected);

            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlGroupRole, Constants.StaticPath.PartialView._PartialTableGroupRole, _srv.GetList(search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
    }
}
