using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.TemplatePage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.TemplatePage.Services
{
    public interface IApiTemplatePageService
    {
        public Response<TemplatePageMeta> GetDefaultMeta(string controller, string action);


    }
}