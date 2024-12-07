using Admin.LogManagement;
using Admin.LogManagement.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using LogManagementManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using X.PagedList;

namespace Admin.LogManagement
{
    #region Define
    public class PageModel
    {

        public PageTitleModel PageTitle = new PageTitleModel("LogManagement", "Danh sách", "fas fa-layer-group", "/LogManagement");



        private string CreateLogManagement = "admin";

        public IPagedList<Database.LogAdminActivity> List { get; set; } 
        public List<Database.LogManagementConfig> Configs { get; set; }
        public Database.LogAdminActivity LogManagement { get; set; }
        public LogManagementDetail LogManagementModel { get; set; }
        public ParamSearch search { get; set; }
        public PaginationModel Pagination = new PaginationModel();


    }
    #endregion

    public partial class LogManagementController
    {
        public PageModel _pageModel = new PageModel();

        public IActionResult Index(ParamSearch search)
        {

            _pageModel.Configs = _rep.GetAllConfigs().Data;

            //search = new ParramSearch();
            //_pageModel.ModelLogManagement = new Database.LogManagement();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.List = _rep.GetList(search).Data;
            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(ParamSearch search)
        {

            _pageModel.List = _rep.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }
        public JsonResult Search(ParamSearch search)
        {
            _pageModel.List = _rep.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(Constants.StaticPath.PartialView._PartialUrl, Constants.StaticPath.PartialView._PartialTable, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }


        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {

            var res = _rep.SaveConfig(formData, tab);

            return new JsonResult(new { response = res });

        }
    }
}