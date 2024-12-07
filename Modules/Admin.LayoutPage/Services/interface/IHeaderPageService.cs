using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.LayoutPage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Services
{
    public interface IHeaderPageService
    {
         public Response<IPagedList<Database.HeaderPage>> GetList(ParamSearch search);
        public Response<Database.HeaderPage> GetById(int id);
        public Response<LayoutPage.Database.HeaderPage> Save(LayoutPage.Database.HeaderPage data );
        public Response Delete(List<int> ids);

        public Response<string> GenerateHeaderHtml(long pid);

    }
}