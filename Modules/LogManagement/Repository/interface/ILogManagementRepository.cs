using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.LogManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.LogManagement
{
    public interface ILogManagementRepository
    {
         public Response<IPagedList<Database.LogAdminActivity>> GetList(ParamSearch search);

        public Response<List<LogManagement.Database.LogManagementConfig>> SaveConfig(IFormCollection formData,string tab);
        public Response<List<LogManagement.Database.LogManagementConfig>> GetAllConfigs();
        public Response<LogManagement.Database.LogManagementConfig> GetConfigByKey(string key);

    }
}