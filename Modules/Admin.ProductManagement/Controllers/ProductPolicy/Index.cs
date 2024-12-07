//using Admin.ProductPolicy.Models;
using Admin.ProductManagement.Models;
using Admin.ProductManagement.Constants;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.Models.ViewModels.ProductPolicy;
using Admin.ProductManagement.DataTransferObjects.ProductPolicy;

namespace Admin.ProductManagement.Controllers
{

    public partial class ProductPolicyController
    {
        #region Define
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Điều khoản & chính sách", "Danh sách", "fas fa-layer-group", "/ProductPolicy/Index");

            public ProductPolicyPagedViewModel Data;
            public List<ProductPolicyConfigDto> Configs;
            public List<ProductPolicyDto> List;
            public ProductPolicyDto EditModel;
            public ProductPolicySearchModel Search;
            public PaginationModel Pagination = new PaginationModel();
        }
        #endregion

        public IActionResult Index()
        {
            _pageModel.Configs = _productPolicyService.GetAllConfigs().Data.Items;

            _pageModel.Search = new ProductPolicySearchModel();
            _pageModel.Pagination = new PaginationModel();

            ProductPolicyPagedViewModel data = _productPolicyService.GetList(_pageModel.Search).Data;
            _pageModel.Data = data;
            _pageModel.List = data.Items;
            _pageModel.Pagination.PageIndex = data.PageNumber;
            _pageModel.Pagination.PageCount = data.PageCount;
            return View(_pageModel);
        }

        public JsonResult GetList(ProductPolicySearchModel search)
        {
            ProductPolicyPagedViewModel data = _productPolicyService.GetList(search).Data;
            _pageModel.List = data.Items;
            return new JsonResult(_pageModel.List);
        }

        [HttpPost]
        public JsonResult Search(ProductPolicySearchModel search)
        {
            ProductPolicyPagedViewModel data = _productPolicyService.GetList(search).Data;
            _pageModel.Data = data;
            _pageModel.List = data.Items;
            string list = _viewRender.RenderPartialViewToString(ProductPolicyConstants.StaticPath.PartialView.Index_Table, _pageModel.Data);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = data.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }

        [HttpPost]
        public JsonResult Delete(List<long> ids)
        {
            _pageModel.Search = new ProductPolicySearchModel();
            var res = _productPolicyService.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(ProductPolicyConstants.StaticPath.PartialView.Index_Table, _productPolicyService.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }

        [HttpPost]
        public JsonResult Enable(List<long> ids, bool isEnable)
        {
            _pageModel.Search = new ProductPolicySearchModel();
            var res = _productPolicyService.Enable(ids, isEnable);
            var listData = _viewRender.RenderPartialViewToString(ProductPolicyConstants.StaticPath.PartialView.Index_Table, _productPolicyService.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }

        public JsonResult Move(int fromId, int toId, int pageIndex)
        {
            _pageModel.Search = new ProductPolicySearchModel();
            _pageModel.Search.PageIndex = pageIndex;
            var res = _productPolicyService.Move(fromId, toId);
            var listData = _viewRender.RenderPartialViewToString(ProductPolicyConstants.StaticPath.PartialView.Index_Table, _productPolicyService.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }

        public JsonResult EnableUpdateOrder(ProductPolicySearchModel search)
        {
            var res = _productPolicyService.EnableUpdateOrder();
            var listData = _viewRender.RenderPartialViewToString(ProductPolicyConstants.StaticPath.PartialView.Index_Table, _productPolicyService.GetList(search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }

        public JsonResult UpdateOrder(int id, double order)
        {
            _pageModel.Search = new ProductPolicySearchModel();
            var res = _productPolicyService.UpdateOrder(id, order);
            return new JsonResult(new { response = res });
        }

        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var res = _productPolicyService.SaveConfig(formData);
            return new JsonResult(new { response = res });
        }
    }
}
