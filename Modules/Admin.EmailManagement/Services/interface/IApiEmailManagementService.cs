using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.EmailManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.EmailManagement.Api.Models;

namespace Admin.EmailManagement.Services
{
    public interface IApiEmailManagementService
    {

        public Response<Api.Models.Response.GetEmailConfig> GetEmailConfig(string code);
        public Response<Api.Models.Response.GetEmailTemplate> GetEmailTemplate(string code);


    }
}