using Admin.ProductManagement.Constants;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects.ProductSpecificaty;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.ProductSpecificaty;
using Admin.ProductManagement.Services;
using ComponentUILibrary.Models;
using ComponentUILibrary.ViewComponents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{
    public partial class ProductSpecificatyController : Controller
    {
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Thuộc tính sản phẩm", "Thông tin api misa", "fas fa-layer-group", "/MisaApi");
            public List<ProductSpecificatyDto> List;
            public ProductSpecificatyPagedViewModel Data;
            public List<ProductSpecificatyConfigDto> Configs;
            public ProductSpecificatyDto EditModel;
            public ProductSpecificatySearchModel Search;
            public string Group { get; set; } = "";

            public PaginationModel Pagination = new PaginationModel();
        }
        [HttpGet("ProductSpecificaty/Index/{Group}/")]
        public IActionResult Index(string Group)
        {
            ViewBag.Group = Group;
            _pageModel.Group = Group;
            _pageModel.Configs = _service.GetAllConfigs().Data.Items;

            _pageModel.Search = new ProductSpecificatySearchModel();
            _pageModel.EditModel = new ProductSpecificatyDto();
            _pageModel.Pagination = new PaginationModel();
            _pageModel.Search.Group = Group;

            _pageModel.Data = _service.GetList(_pageModel.Search).Data;

            _pageModel.List = _pageModel.Data.Items;
            _pageModel.Pagination.PageIndex = _pageModel.Search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.Data.PageCount;
            return View(_pageModel);
        }
        [HttpPost]
        public JsonResult Search(ProductSpecificatySearchModel search)
        {
            _pageModel.Data = _service.GetList(search).Data;
            _pageModel.List = _pageModel.Data.Items;
            string list = _viewRender.RenderPartialViewToString(ProductSpecificatyConstants.ConfigPartial.Index_Table, _pageModel.Data);


            _pageModel.Pagination.PageIndex = search.PageIndex;
            _pageModel.Pagination.PageCount = _pageModel.Data.PageCount;
            string paging = _viewRender.RenderPartialViewToString(PaginationComponentInfo.Path, PaginationComponentInfo.Name, _pageModel.Pagination);
            return new JsonResult(new { list = list, paging = paging });
        }
        public JsonResult SaveConfig(IFormCollection formData, string tab)
        {
            var res = _service.SaveConfig(formData);
            return new JsonResult(new { response = res });
        }

        [HttpPost]
        public JsonResult Delete(List<long> ids)
        {
            _pageModel.Search = new ProductSpecificatySearchModel();
            var res = _service.Delete(ids);

            var listData = _viewRender.RenderPartialViewToString(ProductSpecificatyConstants.ConfigPartial.Index_Table, _service.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = listData });
        }

    }
}
