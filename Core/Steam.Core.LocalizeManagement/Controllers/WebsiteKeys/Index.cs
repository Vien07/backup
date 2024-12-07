//using Steam.Core.LocalizeManagement.Models;
using Steam.Core.LocalizeManagement;
using Steam.Core.LocalizeManagement.Constants;
using Steam.Core.LocalizeManagement.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Steam.Core.LocalizeManagement.Controllers
{
    #region Define
    public class PageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Quản lý thông tin Website", "Danh sách", "fas fa-layer-group", "/LocalizeManagement/Index");




        public IPagedList<Database.LocalizeManagement> List;
        public Dictionary<string,string> Configs;
        public Database.LocalizeManagement EditModel;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();



    }
    #endregion
    public partial class LocalizeManagementController 
    {

        public IActionResult Index(string view ="customkey")
        {
            _pageModel.Configs =_repConfig.GetAllConfigs();

            _pageModel.Search = new ParamSearch();
            _pageModel.EditModel = new Database.LocalizeManagement();
            _pageModel.Pagination = new PaginationModel();
            _pageModel.Search.View = view;
            _pageModel.List = _srv.GetList(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(ParamSearch search)
        {

            _pageModel.List = _srv.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }
        [HttpPost]
        public JsonResult Search(ParamSearch search)
        {
            _pageModel.List = _srv.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(LocalizeManagementConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new ParamSearch();
            var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(LocalizeManagementConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _repLocalizeManagement.Enable(ids, isEnable,CreateUser);
            var listData = _viewRender.RenderPartialViewToString(LocalizeManagementConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId,int pageIndex)
        {
            _pageModel.Search = new ParamSearch();
            _pageModel.Search.PageIndex = pageIndex;
            var res = _repLocalizeManagement.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(LocalizeManagementConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _repLocalizeManagement.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(LocalizeManagementConstants.StaticPath.PartialView.Index_Table, _srv.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repLocalizeManagement.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {

            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());
            var res = _repConfig.SaveConfig(listConfig, tab);
            return new JsonResult(new { response = res });

        }

    }

}
