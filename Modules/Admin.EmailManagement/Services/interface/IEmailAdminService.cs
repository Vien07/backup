using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.EmailManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.EmailManagement.Services
{
    public interface IEmailAdminService
    {
         public Response<IPagedList<Database.EmailAdmin>> GetList(ParamSearch search);
        public Response<Database.EmailAdmin> GetById(long id);
        public Response<EmailManagement.Database.EmailAdmin> Save(EmailManagement.Database.EmailAdmin data);
        public Response Delete(List<int> ids);


    }
}