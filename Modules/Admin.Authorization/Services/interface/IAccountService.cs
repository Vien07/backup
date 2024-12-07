using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using Admin.Authorization.Models;
using Steam.Core.Base.Models;

namespace Admin.Authorization.Services
{
    public interface IAccountService
    {
         public Response<IPagedList<Admin.Authorization.Database.User>> GetList(ParamSearch search);
        Response<List<long>> GetLisPermissionGroup(long pidUser);
        public Response<UserDetail> GetById(int Id);
        public Response<Admin.Authorization.Database.User> Save(AccountModelEdit input);
        public Response Delete(List<int> ids);

        public Response ResetPassword(int pid);

    }
}