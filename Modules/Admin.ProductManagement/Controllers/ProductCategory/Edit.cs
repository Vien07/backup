using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects.ProductCategory;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.ProductCategory;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin.ProductManagement.Controllers
{
    public partial class ProductCategoryController
    {
        public class EditModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Danh mục sản phẩm", "Chi tiết", "", "/MisaProductCategory");
            public ProductCategoryViewModel ProductCategoryModel = new();
            public ProductCategoryDto Detail = new();
            public List<SelectControlData> Parrents = new List<SelectControlData>();
            public List<SelectControlData> ListPageCate = new List<SelectControlData>() /*{ new SelectControlData{ Name="/Product/Cate" , Value= "/Product/Cate" } }*/;
            public List<SelectControlData> ListPageDetail = new List<SelectControlData>() /*{ new SelectControlData { Name = "/Product/Detail", Value = "/Product/Detail" } }*/;
            public ProductCategorySearchModel Search;
        }

        EditModel _editModel = new EditModel();

        [HttpGet("ProductCategory/Edit/{id?}")]

        public IActionResult Edit(int id)
        {
            _editModel.Parrents = _service.GetProductCategoryParent(id).Data;
            _editModel.ListPageCate = _service.GetListTemplatePage("cate").Data;
            _editModel.ListPageDetail = _service.GetListTemplatePage("detail").Data;
            if (id != 0)
            {
                var data = _service.GetById(id).Data;
                if (data != null)
                {
                    _editModel.ProductCategoryModel = data;
                }
            }
            return View(_editModel);
        }
        public ActionResult Save(ProductCategorySaveModel data)
        {
            data.CreateUser = CreateUser;
            data.UpdateUser = CreateUser;
            data.UpdateDate = DateTime.Now;
            _editModel.Search = new();
            var res = _service.Save(data);
            return new JsonResult(new { response = res });
        }
    }
}
