using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.Authorization.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.Authorization.Services
{
    public interface IModuleRoleService
    {
         public Response<IPagedList<Database.ModuleRole>> GetList(ParamSearch search);
         public Response<List<Database.ModuleRole>> GetListNoPagelList(ParamSearch search);
         public Response<List<Database.ModuleRole>> GetListNoPagelListNotContainAnonymousRole(ParamSearch search);
        public Response<IPagedList<Database.ModuleRole>> GetListParent(ParamSearch search);
         public Response<IPagedList<Database.ModuleRole>> GetListChild(ParamSearch search, long? IdParent);
        public Response<ModuleRoleDetail> GetById(int id);
        //public Response<Authorization.Database.ModuleRole> Save(Authorization.Database.ModuleRole data, List<IFormFile> files,string listFiles );
        public Response<Database.ModuleRole> Save(Database.ModuleRole data);
        public Response Delete(List<int> ids);


    }
}