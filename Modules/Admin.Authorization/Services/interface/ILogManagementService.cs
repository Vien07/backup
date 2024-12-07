using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.Authorization.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.Authorization.Services
{
    public interface ILogManagementService
    {
         public Response<IPagedList<Database.LogManagement>> GetList(ParamSearch search);
        public Response<Authorization.Database.LogManagement> Save(LogManagementModelEdit input);


    }
}