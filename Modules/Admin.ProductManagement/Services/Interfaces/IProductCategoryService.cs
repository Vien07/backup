using Microsoft.AspNetCore.Http;
using ComponentUILibrary.Models;
using Steam.Core.Base.Models;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.ProductCategory;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.DataTransferObjects.ProductCategory;

namespace Admin.ProductManagement.Services
{
    public interface IProductCategoryService
    {
        Response<ProductCategoryPagedViewModel> GetList(ProductCategorySearchModel search);
        Response<ProductCategoryViewModel> GetById(long id);
        Response<ProductCategoryViewModel> Save(ProductCategorySaveModel data);
        Response Delete(long id);

        Response<List<SelectControlData>> GetProductCategoryParent(long id);
        Response<List<SelectControlData>> GetProductCategoryTreeChildrenByParentId(long parentId);
        Response<List<SelectControlData>> GetListTemplatePage(string type);
        List<ProductCategoryDto> GetChildrenPostCategory(long parentId);
        Response<string> GenerateXMLRewriteUrl();
    }
}