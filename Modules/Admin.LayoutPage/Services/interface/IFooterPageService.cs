using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.LayoutPage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Services
{
    public interface IFooterPageService
    {
         public Response<IPagedList<Database.FooterPage>> GetList(ParamSearch search);
        public Response<Database.FooterPage> GetById(int id);
        public Response<Database.FooterItem> GetChildById(int id);
        public Response<Database.FooterPage> Save(FooterPageModelEdit data );
        public Response<Database.FooterItem> SaveChild(FooterItemModelEdit data );
        public Response DeleteChild(List<int> ids);
        public Response Delete(List<int> ids);

        public Response<List<Database.FooterItem>> GetChildList(long? Pid);
        public Response<string> GenerateFooterHtml(long pid);

    }
}