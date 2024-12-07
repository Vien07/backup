//using Admin.FooterPage.Models;
using Admin.LayoutPage;
using Admin.LayoutPage.Constants;
using Admin.LayoutPage.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.LayoutPage.Controllers
{
    #region Define
    public class FooterPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Footer", "Danh sách", "fas fa-layer-group", "/FooterPage");




        public IPagedList<Database.FooterPage> List;
        public Dictionary<string, string> Configs;
        public Database.FooterPage Detail;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();



    }
    #endregion

    public partial class FooterPageController
    {

        public IActionResult Index()
        {
            _pageModel.Configs = _repFooterPageConfig.GetAllConfigs();

            _pageModel.Search = new ParamSearch();
            _pageModel.Detail = new Database.FooterPage();
            _pageModel.Pagination = new PaginationModel();

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
            string list = _viewRender.RenderPartialViewToString(FooterPageConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            ParamSearch search = new ParamSearch();
            //_pageModel.Search = new ParamSearch();
            var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(FooterPageConstants.StaticPath.PartialView.Index_Table, _srv.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _repFooterPage.Enable(ids, isEnable,CreateUser);
            var listData = _viewRender.RenderPartialViewToString(FooterPageConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repFooterPage.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(FooterPageConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _repFooterPage.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(FooterPageConstants.StaticPath.PartialView.Index_Table, _srv.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repFooterPage.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {

            var listConfig = formData.ToDictionary(x => x.Key, x => x.Value.ToString());
            var res = _repFooterPageConfig.SaveConfig(listConfig, tab);

            return new JsonResult(new { response = res });

        }

    }

}
