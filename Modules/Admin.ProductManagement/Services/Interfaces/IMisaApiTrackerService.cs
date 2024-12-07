using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.ProductManagement.Models.SearchModels;
using Admin.ProductManagement.Models.ViewModels.MisaApiTracker;

namespace Admin.ProductManagement.Services
{
    public interface IMisaApiTrackerService
    {
        Response<MisaApiTrackerPagedViewModel> GetList(MisaApiTrackerSearchModel search);
        Response<MisaApiTrackerViewModel> GetById(long id);
        Response<MisaApiConfigViewModel> SaveConfig(IFormCollection formData);
        Response<MisaApiConfigViewModel> GetAllConfigs();
        Response<MisaApiConfigViewModel> GenerateAccessToken();
    }
}