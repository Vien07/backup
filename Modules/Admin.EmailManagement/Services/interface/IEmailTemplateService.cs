using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.EmailManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.EmailManagement.Services
{
    public interface IEmailTemplateService
    {
         public Response<IPagedList<Database.EmailTemplate>> GetList(ParamSearch search);
        public Response<EmailTemplateDetail> GetById(int id);
        public Response<EmailManagement.Database.EmailTemplate> Save(EmailManagement.Database.EmailTemplate data);
        public Response Delete(List<int> ids);


    }
}