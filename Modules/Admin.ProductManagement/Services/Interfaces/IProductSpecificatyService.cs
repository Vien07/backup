using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.ProductManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.ProductSpecificaty;
using Admin.ProductManagement.Models.SaveModels;

namespace Admin.ProductManagement.Services
{
    public interface IProductSpecificatyService
    {
        Response<ProductSpecificatyPagedViewModel> GetList(ProductSpecificatySearchModel search);
        Response<ProductSpecificatyViewModel> GetById(long id);
        Response<ProductSpecificatyViewModel> Save(ProductSpecificatySaveModel data);
        Response<ProductSpecificatyConfigViewModel> SaveConfig(IFormCollection formData);
        Response<ProductSpecificatyConfigViewModel> GetAllConfigs();
        Response Delete(List<long> ids);
    }
}