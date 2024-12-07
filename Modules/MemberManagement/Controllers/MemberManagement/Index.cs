using Admin.MemberManagement;
using Admin.MemberManagement.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using MemberManagementManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using X.PagedList;
using PartialViews = Admin.MemberManagement.Constants.StaticPath.PartialView;

namespace Admin.MemberManagement.Controllers
{
    #region Define
    public class PageModel
    {

        public PageTitleModel PageTitle = new PageTitleModel("Quản lý thành viên", "Danh sách", "fas fa-layer-group", "/MemberManagement");



        private string CreateMemberManagement = "admin";

        public IPagedList<Database.MemberManagement> List { get; set; } 
        public Dictionary<string,string> Configs { get; set; }
        public Database.MemberManagement MemberManagement { get; set; }
        public MemberManagementDetail MemberManagementModel { get; set; }
        public ParamSearch search { get; set; }
        public PaginationModel Pagination = new PaginationModel();


    }
    #endregion

    public partial class MemberManagementController
    {
        public PageModel _pageModel = new PageModel();

        public IActionResult Index(ParamSearch search)
        {

            _pageModel.Configs = _repConfig.GetAllConfigs();

            //search = new ParramSearch();
            //_pageModel.ModelMemberManagement = new Database.MemberManagement();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.List = _srv.GetList(search).Data;
            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(ParamSearch search)
        {

            _pageModel.List = _srv.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }
        public JsonResult Search(ParamSearch search)
        {
            _pageModel.List = _srv.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.search = new ParamSearch();
            var res = _srv.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _srv.GetList(_pageModel.search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.search = new ParamSearch();
            var res = _repMemberManagement.Enable(ids, isEnable,CreateUser);
            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _srv.GetList(_pageModel.search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.search = new ParamSearch();

            var res = _repMemberManagement.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _srv.GetList(_pageModel.search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _repMemberManagement.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(PartialViews.Index_Table, _srv.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.search = new ParamSearch();

            var res = _repMemberManagement.UpdateOrder(id, order);
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