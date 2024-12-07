using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.ProductManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.OrderManagement;

namespace Admin.ProductManagement.Services
{
    public interface IOrderManagementService
    {
        Response<OrderManagementPagedViewModel> GetList(OrderManagementSearchModel search);
        Response<OrderManagementConfigViewModel> SaveConfig(IFormCollection formData, string group);
        Response<OrderManagementConfigViewModel> GetAllConfigs();
        Response<OrderManagementConfigViewModel> GetConfigByKey(string key);
    }
}