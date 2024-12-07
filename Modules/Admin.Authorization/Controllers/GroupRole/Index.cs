
using Admin.Authorization.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using PartialViews = Admin.Authorization.Constants.StaticPath.PartialViewGroupRole;

namespace Admin.Authorization.Controllers
{
    public class GroupRoleIndexPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Quản lý nhóm quyền", "Danh sách", "fas fa-layer-group", "/GroupRole");

        public IPagedList<Database.GroupRole> List;
        public Dictionary<string,string> Configs;
        public Database.GroupRole ModelAuthorization;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();
    }
    public partial class GroupRoleController
    {
        GroupRoleIndexPageModel _pageModel = new GroupRoleIndexPageModel();

        public IActionResult Index()
        {
            _pageModel.Configs = _repConfig.GetAllConfigs();

            _pageModel.Search = new ParamSearch();
            _pageModel.ModelAuthorization = new Database.GroupRole();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.List = _srv.GetList(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult List(ParamSearch Search)
        {

            _pageModel.List = _srv.GetList(Search).Data;
            return new JsonResult(_pageModel.List);

        }

        [HttpPost]
        public JsonResult Search(ParamSearch Search)
        {
            _pageModel.List = _srv.GetList(Search).Data;
            string list = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlGroupRole, Constants.StaticPath.PartialView._PartialTableGroupRole, _pageModel.List);


            _pageModel.Pagination.PageIndex = Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }

        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new ParamSearch();
            var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlGroupRole, Constants.StaticPath.PartialView._PartialTableGroupRole, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }

        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _repGroupRoles.Enable(ids, isEnable,CreateUser);
            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlGroupRole, Constants.StaticPath.PartialView._PartialTableGroupRole, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repGroupRoles.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlGroupRole, Constants.StaticPath.PartialView._PartialTableGroupRole, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch Search)
        {
            //Search = new ParamSearch();

            var res = _repGroupRoles.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlGroupRole, Constants.StaticPath.PartialView._PartialTableGroupRole, _srv.GetList(Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repGroupRoles.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }

        [HttpPost]
        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {

            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());
            var res = _repConfig.SaveConfig(listConfig, tab);
            return new JsonResult(new { response = res });

        }

    }

}
