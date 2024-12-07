using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects.Product;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.Product;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{
    public partial class ProductController
    {
        #region Define
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Sản phẩm", "Danh sách", "fas fa-layer-group", "/MisaProduct");
            public ProductPagedViewModel Data;
            public List<ProductDto> List;
            public List<ProductConfigDto> Configs;
            public ProductDto EditModel;
            public ProductSearchModel Search;
            public PaginationModel Pagination = new PaginationModel();
        }
        #endregion

        public PageModel _pageModel = new PageModel();

        public IActionResult Index()
        {
            _pageModel.Configs = _service.GetAllConfigs().Data.Items;

            _pageModel.Search = new ProductSearchModel();
            _pageModel.EditModel = new ProductDto();
            _pageModel.Pagination = new PaginationModel();

            _pageModel.Data = _service.GetList(_pageModel.Search).Data;
            _pageModel.List = _pageModel.Data.Items;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.Data.PageCount;
            return View(_pageModel);
        }

        public JsonResult GetList(ProductSearchModel search)
        {
            _pageModel.List = _service.GetList(search).Data.Items;
            return new JsonResult(_pageModel.List);
        }

        [HttpPost]
        public JsonResult ExcuteSyncMisa(SyncMisaProductSearchModel param)
        {
            _pageModel.Search = new ProductSearchModel();
            var result = _service.SyncMisa(param);
            var listData = _viewRender.RenderPartialViewToString(ProductConstants.ConfigPartial.Index_Table, _service.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = result, listData = listData });
        }

        [HttpPost]
        public JsonResult Search(ProductSearchModel search)
        {
            _pageModel.Data = _service.GetList(search).Data;
            _pageModel.List = _pageModel.Data.Items;
            string list = _viewRender.RenderPartialViewToString(ProductConstants.ConfigPartial.Index_Table, _pageModel.Data);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.Data.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        [HttpPost]
        public JsonResult Delete(List<long> ids)
        {
            _pageModel.Search = new ProductSearchModel();
            var res = _service.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(ProductConstants.ConfigPartial.Index_Table, _service.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ProductSearchModel();
            var res = _service.Enable(ids, isEnable);
            var listData = _viewRender.RenderPartialViewToString(ProductConstants.ConfigPartial.Index_Table, _service.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult OnGetMove(int fromId, int toId)
        {
            _pageModel.Search = new ProductSearchModel();

            var res = _service.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(ProductConstants.ConfigPartial.Index_Table, _service.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult EnableUpdateOrder(ProductSearchModel search)
        {
            var res = _service.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(ProductConstants.ConfigPartial.Index_Table, _service.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
        public JsonResult OnGetUpdateOrder(int id, double order)
        {
            _pageModel.Search = new ProductSearchModel();
            var res = _service.UpdateOrder(id, order);
            return new JsonResult(new { response = res });
        }
        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var res = _service.SaveConfig(formData);
            return new JsonResult(new { response = res });
        }
    }
}
