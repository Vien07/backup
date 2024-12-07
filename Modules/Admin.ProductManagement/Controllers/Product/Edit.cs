using Admin.ProductManagement.Constants;
using Admin.ProductManagement.DataTransferObjects.Product;
using Admin.ProductManagement.Models;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.ProductPolicy;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Admin.PostsManagement.Models;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{
    public partial class ProductController
    {
        public class EditModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Sản phẩm", "Chi tiết", "fas fa-layer-group", "/MisaProduct");
            public ProductDto Detail = new();
            public List<ProductFileDto> ListFiles = new();
            public List<ProductDetailDto> ProductChildren;
            public List<SelectControlData> ListProductTypes = new List<SelectControlData>();
            public List<ProductPolicyWithGroupViewModel> ListPolicies = new List<ProductPolicyWithGroupViewModel>();
            public PostsManagement_List ListPost;
            public PaginationModel ListPostPagination = new PaginationModel();
            public PostsManagementModel.ParamSearch PostSearch;
            public string? ProductTypes { get; set; }
            public void InitListProductTypesData()
            {
                try
                {
                    var listTemp = this.ProductTypes.Split(';');
                    foreach (var item in listTemp)
                    {
                        var tempType = item.Split(":");
                        if (tempType.Length == 2)
                        {
                            this.ListProductTypes.Add(new SelectControlData() { Name = tempType[0], Value = tempType[1], Order = 0 });
                        }
                    }
                }
                catch (Exception)
                {
                    this.ListProductTypes = new List<SelectControlData>();
                }
            }
        }

        EditModel _editModel = new EditModel();

        [HttpGet("Product/Edit/{id?}")]
        public IActionResult Edit(int id)
        {
             var config = _service.GetAllConfigs().Data.Items;
            _editModel.ProductTypes = config.Where(p => p.Key == ProductConstants.ConfigAdmin.ProductTypes).FirstOrDefault().Value;

            _editModel.InitListProductTypesData();
            _editModel.ListPolicies= _repPolicy.GetListWithGroup();
            _editModel.PostSearch = new PostsManagementModel.ParamSearch();
            _editModel.ListPost = _postService.GetList(_editModel.PostSearch).Data;
            _editModel.ListPostPagination.PageIndex = _editModel.PostSearch.PageIndex;
            _editModel.ListPostPagination.PageCount = _editModel.ListPost.PageCount;
            if (id != 0)
            {
                var data = _service.GetById(id).Data;
                if (data != null)
                {
                    _editModel.Detail = data.Detail;
                    _editModel.ListFiles = data.Files;
                    _editModel.ProductChildren = data.ProductChildren;
                }
            }
            return View(_editModel);
        }

        [HttpPost]
        public ActionResult Save(ProductSaveModel data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            data.UpdateDate = DateTime.Now;

            _pageModel.Search = new ProductSearchModel();
            var res = _service.Save(data);

            var listData = _viewRender.RenderPartialViewToString(ProductConstants.ConfigPartial.Index_Table, _service.GetList(_pageModel.Search).Data);

            return new JsonResult(new { response = res, listData = listData });
        }

        [HttpPost]
        public JsonResult DeleteProductDetail(List<long> ids, long parentId)
        {
            var res = _service.DeleteProductDetail(ids);

            var listData = _viewRender.RenderPartialViewToString(ProductConstants.ConfigPartial.Index_TableProductChildren, _service.GetProductDetailList(parentId).Data);
            return new JsonResult(new { response = res, listData = listData });
        }
    }
}
