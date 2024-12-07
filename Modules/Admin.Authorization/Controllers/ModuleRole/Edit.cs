using Admin.Authorization.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Admin.Authorization.Controllers
{
    public class ModuleRoleEditPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Định nghĩa Module", "Chi tiết", "", "/ModuleRole");
        public long? idParentRoot;

        public ModuleRoleDetail ModuleRoleModel = new ModuleRoleDetail();
        public Database.ModuleRole PermissionModel = new Database.ModuleRole();
        public ParamSearch Search;
        public IPagedList<Database.ModuleRole> List;
        public PaginationModel Pagination = new PaginationModel();
    }
    public partial class ModuleRoleController
    {
        ModuleRoleEditPageModel _editModel = new ModuleRoleEditPageModel();
        [HttpGet("ModuleRole/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
            ModuleRoleEditPageModel _editModel = new ModuleRoleEditPageModel();
            if (id != 0)
            {
                var data = _srv.GetById(id).Data;
                if (data != null)
                {
                    _editModel.ModuleRoleModel = data;
                    _editModel.PermissionModel.IdParent = _editModel.ModuleRoleModel.Detail.Pid;
                }
                _editModel.idParentRoot = id;
                ParamSearch param = new ParamSearch();
                _editModel.List = _srv.GetListChild(param, id).Data;
                _editModel.Pagination.PageIndex = param.PageIndex;
                _editModel.Pagination.PageCount = _editModel.List.PageCount;
            }

            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(Admin.Authorization.Database.ModuleRole data, string CheckBoxAllowAnonymous, string Log, string LogChild, string CheckBoxAllowAnonymousChild)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;

            if (data.IdParent == null)
            {
                data.IdParent = _editModel.idParentRoot;
            }
            if (CheckBoxAllowAnonymous != null)
            {
                data.AllowAnonymous = int.Parse(CheckBoxAllowAnonymous) == 1 ? true : false;
            }
            if (CheckBoxAllowAnonymousChild != null)
            {
                data.AllowAnonymous = int.Parse(CheckBoxAllowAnonymousChild) == 1 ? true : false;
            }
            if (Log != null)
            {
                data.Log = int.Parse(Log) == 1 ? true : false;
            }
            if (LogChild != null)
            {
                data.Log = int.Parse(LogChild) == 1 ? true : false;
            }
            //if (niceSelectIdParent != null)
            //{
            //    data.IdParent = long.Parse(niceSelectIdParent);
            //}
            _editModel.Search = new ParamSearch();

            var res = _srv.Save(data);
            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialTableModuleRoleChild, _srv.GetList(_editModel.Search).Data);

            if (data.IdParent != 0)
            {
                listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialTableModuleRoleChild, _srv.GetListChild(_editModel.Search, data.IdParent).Data);


            }


            return new JsonResult(new { response = res, listData = listData });
        }

        [HttpGet]
        public JsonResult EditPermission(int idPermission)
        {

            try
            {
                var data = _srv.GetById(idPermission).Data;
                if (data.Detail != null)
                {
                    _editModel.PermissionModel = data.Detail;

                }
                var modalHtml = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialModelModuleRole, _editModel.PermissionModel);

                return new JsonResult(new { response = modalHtml });

            }
            catch (Exception)
            {
                var modalHtml = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialModelModuleRole, _editModel.PermissionModel);

                return new JsonResult(new { response = modalHtml });
            }
        }

        [HttpPost]
        public JsonResult DeleteChild(List<int> ids, long parrentID)
        {
            _editModel.Search = new ParamSearch();
            var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialTableModuleRoleChild, _srv.GetListChild(_editModel.Search, parrentID).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        public ActionResult NewPermissionModal()
        {

            _editModel.Search = new ParamSearch();


            var modalHtml = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialModelModuleRole, _editModel.PermissionModel);

            return new JsonResult(new { modal = modalHtml });
        }
    }
}
