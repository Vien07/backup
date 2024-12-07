//using Admin.HomePage.Models;
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
    public class PageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Cấu hình trang chủ", "Danh sách", "fas fa-layer-group", "/HomePage");




        public IPagedList<Database.HomePage> List;
        public Dictionary<string, string> Configs;
        public Database.HomePage EditModel;
        public HomePageModel.ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();



    }
    #endregion
    public partial class HomePageController 
    {

        public IActionResult Index()
        {
            _pageModel.Configs = _repConfig.GetAllConfigs();

            _pageModel.Search = new HomePageModel.ParamSearch();
            _pageModel.EditModel = new Database.HomePage();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.List = _srv.GetList(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(HomePageModel.ParamSearch search)
        {

            _pageModel.List = _srv.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }
        [HttpPost]
        public JsonResult Search(HomePageModel.ParamSearch search)
        {
            _pageModel.List = _srv.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(HomePageConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new HomePageModel.ParamSearch();
            var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(HomePageConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new HomePageModel.ParamSearch();
            var res = _repoHomePage.Enable(ids, isEnable,CreateUser);
            var listData = _viewRender.RenderPartialViewToString(HomePageConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.Search = new HomePageModel.ParamSearch();

            var res = _repoHomePage.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(HomePageConstants.StaticPath.PartialView.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(HomePageModel.ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _repoHomePage.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(HomePageConstants.StaticPath.PartialView.Index_Table, _srv.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new HomePageModel.ParamSearch();

            var res = _repoHomePage.UpdateOrder(id, order);
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
