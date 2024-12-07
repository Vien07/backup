//using Admin.Slider.Models;
using Admin.Slider;
using Admin.Slider.Constants;
using Admin.Slider.Models;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.Slider
{
    #region Define
    public class PageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Slider", "Danh sách", "fas fa-layer-group", "/Slider/Index");




        public IPagedList<Database.Slider> List;
        public List<Database.SliderConfig> Configs;
        public Database.Slider EditModel;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();



    }
    #endregion
    public partial class SliderController 
    {

        public IActionResult Index()
        {
            _pageModel.Configs = _rep.GetAllConfigs().Data;

            _pageModel.Search = new ParamSearch();
            _pageModel.EditModel = new Database.Slider();
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
            string list = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Index_Table, _pageModel.List);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.List.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<int> ids)
        {
            _pageModel.Search = new ParamSearch();
            var res = _rep.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<int> ids, bool isEnable)
        {
            _pageModel.Search = new ParamSearch();
            var res = _rep.Enable(ids, isEnable);
            var listData = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult Move(int fromId, int toId,int pageIndex)
        {
            _pageModel.Search = new ParamSearch();
            _pageModel.Search.PageIndex = pageIndex;
            var res = _rep.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });

        }
        public JsonResult EnableUpdateOrder(ParamSearch search)
        {
            //search = new ParamSearch();

            var res = _rep.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(SliderConstants.StaticPath.PartialView.Index_Table, _rep.GetList(search).Data);
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
