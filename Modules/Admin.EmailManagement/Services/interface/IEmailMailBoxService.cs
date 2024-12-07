using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.EmailManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.EmailManagement.Servcies
{
    public interface IEmailMailBoxService
    {
         public Response<IPagedList<Database.EmailMailBox>> GetList(ParamSearch search);
        public Response<Database.EmailMailBox> GetById(int id);
        public Response<EmailManagement.Database.EmailMailBox> Save(EmailManagement.Database.EmailMailBox data);
        public Response Delete(List<int> ids);

        public Response ReSendEmail(int id);
    }
}