//using Admin.Collection.Models;
using Admin.Collection;
using Admin.Collection.Constants;
using Admin.Collection.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.Collection
{
    public partial class CollectionController 
    {

        public IActionResult Index()
        {
            _pageModel.Configs = _rep.GetAllConfigs().Data;

            _pageModel.Search = new CollectionModel.ParamSearch();
            _pageModel.EditModel = new Database.Collection();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.List = _rep.GetList(_pageModel.Search).Data;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            return View(_pageModel);
        }
        public JsonResult GetList(CollectionModel.ParamSearch search)
        {

            _pageModel.List = _rep.GetList(search).Data;
            return new JsonResult(_pageModel.List);

        }
        [HttpPost]
        public JsonResult Search(CollectionModel.ParamSearch search)
        {
            _pageModel.List = _rep.GetList(search).Data;
            string list = _viewRender.RenderPartialViewToString(CollectionConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new CollectionModel.ParamSearch();
            var res = _rep.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(CollectionConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<int> ids, bool isEnable)
        {
            _pageModel.Search = new CollectionModel.ParamSearch();
            var res = _rep.Enable(ids, isEnable);
            var listData = _viewRender.RenderPartialViewToString(CollectionConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult OnGetMove(int fromId, int toId)
        {
            _pageModel.Search = new CollectionModel.ParamSearch();

            var res = _rep.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(CollectionConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(CollectionModel.ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _rep.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(CollectionConstants.StaticPath.PartialView.Index_Table, _rep.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult OnGetUpdateOrder(int id, double order)
        {
            _pageModel.Search = new CollectionModel.ParamSearch();

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
