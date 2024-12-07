//using Admin.HeaderPage.Models;
using Admin.HeaderPage;
using Admin.HeaderPage.Constants;
using Admin.HeaderPage.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.HeaderPage
{
    public partial class MenuStyleController
    {
        #region Define
        public class MenuStyleModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Định dạng Menu", "Danh sách", "fas fa-layer-group", "/MenuStyle");




            public IPagedList<Database.MenuStyle> List;
            public List<Database.MenuStyleConfig> Configs;
            public Database.MenuStyle Detail;
            public ParamSearch Search;
            public PaginationModel Pagination = new PaginationModel();



        }
        #endregion

        public IActionResult Index()
        {
            _pageModel.Configs = _rep.GetAllConfigs().Data;

            _pageModel.Search = new ParamSearch();
            _pageModel.Detail = new Database.MenuStyle();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.List = _rep.GetList(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(ParamSearch search)
        {

            _pageModel.List = _rep.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }
        [HttpPost]
        public JsonResult Search(ParamSearch search)
        {
            _pageModel.List = _rep.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(HeaderPageConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


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
            var res = _rep.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(MenuStyleConstants.StaticPath.PartialView.Index_Table, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<int> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _rep.Enable(ids, isEnable);
            var listData = _viewRender.RenderPartialViewToString(MenuStyleConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId)
        {
            _pageModel.Search = new ParamSearch();

            var res = _rep.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(MenuStyleConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _rep.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(HeaderPageConstants.StaticPath.PartialView.Index_Table, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ParamSearch();

            var res = _rep.UpdateOrder(id, order);
            return new JsonResult(new { response = res });

        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {

            var res = _rep.SaveConfig(formData, tab);

            return new JsonResult(new { response = res });

        }

    }

}
