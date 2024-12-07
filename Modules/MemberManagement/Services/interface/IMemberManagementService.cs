using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.MemberManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.MemberManagement.Services
{
    public interface IMemberManagementService
    {
         public Response<IPagedList<Database.MemberManagement>> GetList(ParamSearch search);
        public Response<MemberManagementDetail> GetById(int id);
        public Response<MemberManagement.Database.MemberManagement> Save(MemberManagement.Database.MemberManagement data, List<IFormFile> files,string listFiles );
        public Response Delete(List<int> ids);

        public Response ResetPassword(int ids);


    }
}