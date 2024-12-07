using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.TemplatePage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.TemplatePage.Services
{
    public interface ITemplatePageService
    {
         public Response<IPagedList<Database.TemplatePage>> GetList(ParamSearch search);
        public Response<TemplatePageDetail> GetById(int id);
        public Response<TemplatePage.Database.TemplatePage> Save(TemplatePageModelEdit input);
        public Response Delete(List<int> ids);
       

    }
}