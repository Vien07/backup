using Microsoft.AspNetCore.Http;
using Admin.ProductManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.SaveModels;
using Admin.ProductManagement.Models.ViewModels.ProductPolicy;

namespace Admin.ProductManagement.Services
{
    public interface IProductPolicyService
    {
        Response<ProductPolicyPagedViewModel> GetList(ProductPolicySearchModel search);
        Response<ProductPolicyViewModel> GetById(int id);
        Response<ProductPolicyViewModel> Save(ProductPolicySaveModel saveModel);
        Response Delete(List<long> ids);
        Response Enable(List<long> ids, bool isEnable);
        Response EnableUpdateOrder();
        Response UpdateOrder(int id, double order);
        Response Move(int fromId, int toId);
        Response<ProductPolicyConfigViewModel> SaveConfig(IFormCollection formData);
        Response<ProductPolicyConfigViewModel> GetAllConfigs();
        Response<ProductPolicyConfigViewModel> GetConfigByKey(string key);
        List<ProductPolicyWithGroupViewModel> GetListWithGroup();
    }
}