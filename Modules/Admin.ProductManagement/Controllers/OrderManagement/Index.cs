using Admin.ProductManagement.Constants;
using Admin.ProductManagement.DataTransferObjects.OrderManagement;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.OrderManagement;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.ProductManagement.Controllers
{
    public partial class OrderManagementController
    {
        #region Define
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("OrderManagement", "Danh sách", "fas fa-layer-group", "/OrderManagement/Index");
            public List<OrderManagementDto> List;
            public OrderManagementPagedViewModel Data;
            public List<OrderManagementConfigDto> Configs;
            public OrderManagementDto EditModel;
            public OrderManagementSearchModel Search;
            public PaginationModel Pagination = new PaginationModel();
        }
        #endregion
        public IActionResult Index()
        {
            _pageModel.Configs = _service.GetAllConfigs().Data.Items;

            _pageModel.Search = new OrderManagementSearchModel();
            _pageModel.EditModel = new OrderManagementDto();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.Data = _service.GetList(_pageModel.Search).Data;
            _pageModel.List = _pageModel.Data.Items;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.Data.PageCount;
            return View(_pageModel);
        }

        public JsonResult GetList(OrderManagementSearchModel search)
        {
            _pageModel.Data = _service.GetList(search).Data;
            _pageModel.List = _pageModel.Data.Items;
            return new JsonResult(_pageModel.List);
        }

        [HttpPost]
        public JsonResult Search(OrderManagementSearchModel search)
        {
            _pageModel.Data = _service.GetList(search).Data;
            _pageModel.List = _pageModel.Data.Items;
            string list = _viewRender.RenderPartialViewToString(OrderManagementConstants.StaticPath.PartialView.Index_Table, _pageModel.Data);

            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.Data.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var res = _service.SaveConfig(formData, tab);
            return new JsonResult(new { response = res });
        }
    }
}
