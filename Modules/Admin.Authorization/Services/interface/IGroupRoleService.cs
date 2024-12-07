using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.Authorization.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.Authorization.Services
{
    public interface IGroupRoleService
    {
         public Response<IPagedList<Database.GroupRole>> GetList(ParamSearch search);
         public Response<List<long>> GetListPermission(long pidGroup);
        public Response<GroupRoleDetail> GetById(int id);
        //public Response<Authorization.Database.GroupRole> Save(Authorization.Database.GroupRole data, List<IFormFile> files,string listFiles );
        public Response<Database.GroupRole> Save(Database.GroupRole data, long[] listRole);
        public Response Delete(List<int> ids);

    }
}