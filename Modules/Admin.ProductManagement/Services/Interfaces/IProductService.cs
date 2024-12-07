using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Models;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.ViewModels.Product;

namespace Admin.ProductManagement.Services
{
    public interface IProductService
    {
        Response<ProductPagedViewModel> GetList(ProductSearchModel search);
        Response<ProductDetailPagedViewModel> GetProductDetailList(long parentId);
        Response<ProductViewModel> GetById(long id);

        Response<ProductViewModel> Save(ProductSaveModel data);

        Response Delete(List<long> ids);
        Response DeleteProductDetail(List<long> ids);

        Response Enable(List<long> ids, bool isEnable);
        Response EnableUpdateOrder();
        Response UpdateOrder(int id, double order);
        Response Move(int fromId, int toId);

        Response<ProductConfigViewModel> SaveConfig(IFormCollection formData);
        Response<ProductConfigViewModel> GetAllConfigs();
        Response<ProductConfigViewModel> GetConfigByKey(string key);
        Response<bool> SyncMisa(SyncMisaProductSearchModel param);
    }
}