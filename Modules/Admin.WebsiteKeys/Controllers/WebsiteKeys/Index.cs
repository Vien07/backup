//using Admin.WebsiteKeys.Models;
using Admin.WebsiteKeys;
using Admin.WebsiteKeys.Constants;
using Admin.WebsiteKeys.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.WebsiteKeys.Controllers
{
    #region Define
    public class PageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Quản lý thông tin Website", "Danh sách", "fas fa-layer-group", "/WebsiteKeys/Index");




        public IPagedList<Database.WebsiteKeys> List;
        public Dictionary<string, string> Configs;
        public Database.WebsiteKeys EditModel;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();



    }
    #endregion
    public partial class WebsiteKeysController 
    {

        public IActionResult Index(string view ="customkey")
        {
            _pageModel.Configs = _repWebsiteKeysConfig.GetAllConfigs();

            _pageModel.Search = new ParamSearch();
            _pageModel.EditModel = new Database.WebsiteKeys();
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
            string list = _viewRender.RenderPartialViewToString(WebsiteKeysConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


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

            var listData = _viewRender.RenderPartialViewToString(WebsiteKeysConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _repoWebsiteKeys.Enable(ids, isEnable, CreateUser);
            var listData = _viewRender.RenderPartialViewToString(WebsiteKeysConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId,int pageIndex)
        {
            _pageModel.Search = new ParamSearch();
            _pageModel.Search.PageIndex = pageIndex;
            var res = _repoWebsiteKeys.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(WebsiteKeysConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _repoWebsiteKeys.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(WebsiteKeysConstants.StaticPath.PartialView.Index_Table, _srv.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repoWebsiteKeys.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());
            var res = _repWebsiteKeysConfig.SaveConfig(listConfig, tab);

            return new JsonResult(new { response = res });

        }

    }

}
