
using Admin.Authorization.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using PartialViews = Admin.Authorization.Constants.StaticPath.PartialView;

namespace Admin.Authorization.Controllers
{

    public class AccountIndexPageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Quản lý tài khoản", "Danh sách", "fas fa-layer-group", "/Account");

        public IPagedList<Admin.Authorization.Database.User> List;
        public Dictionary<string,string> Configs;
        public Admin.Authorization.Database.User ModelAccount;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();
    }
    public partial class AccountController
    {
        AccountIndexPageModel _pageModel = new AccountIndexPageModel();

        public IActionResult Index()
        {
            _pageModel.Configs = _repConfig.GetAllConfigs();

            _pageModel.Search = new ParamSearch();
            _pageModel.ModelAccount = new Database.User();
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
            string list = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }

        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new ParamSearch();
            var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }

        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _repUser.Enable(ids, isEnable,CreateUser);
            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _srv.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repUser.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _srv.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch Search)
        {
            //Search = new ParamSearch();

            var res = _repUser.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _srv.GetList(Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();

            var res = _repUser.UpdateOrder(id, order);
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
