
using Admin.Authorization.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.Authorization.Controllers
{
    public class ModuleRoleIndexPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Định nghĩa Module", "Danh sách", "fas fa-layer-group", "/ModuleRole");

        public IPagedList<Database.ModuleRole> List;
        public Dictionary<string,string> Configs;
        public Database.ModuleRole ModelAuthorization;
        public ParamSearch Search = new ParamSearch();
        public PaginationModel Pagination = new PaginationModel();
    }
    public partial class ModuleRoleController
    {
        ModuleRoleIndexPageModel _pageModel = new ModuleRoleIndexPageModel();

        public IActionResult Index()
        {
            _pageModel.Configs = _repConfig.GetAllConfigs();

            //Search = new ParramSearch();
            _pageModel.ModelAuthorization = new Database.ModuleRole();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.List = _srv.GetListParent(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(ParamSearch Search)
        {

            _pageModel.List = _srv.GetListParent(Search).Data;
            return new JsonResult(_pageModel.List);

        }

        [HttpPost]
        public JsonResult Search(ParamSearch Search)
        {
            _pageModel.List = _srv.GetListParent(Search).Data;
            string list = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialTableModuleRole, _pageModel.List);


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

            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialTableModuleRole, _srv.GetListParent(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }

        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _repModuleRole.Enable(ids, isEnable,CreateUser);
            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialTableModuleRole, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repModuleRole.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialTableModuleRole, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch Search)
        {
            //Search = new ParamSearch();

            var res = _repModuleRole.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrlModuleRole, Constants.StaticPath.PartialView._PartialTableModuleRole, _srv.GetList(Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repModuleRole.UpdateOrder(id, order);
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
