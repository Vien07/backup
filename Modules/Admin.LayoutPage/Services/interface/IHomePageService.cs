using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.LayoutPage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Services
{
    public interface IHomePageService
    {
         public Response<IPagedList<Database.HomePage>> GetList(HomePageModel.ParamSearch search);
        public Response<HomePageModel.HomePageDetail> GetById(int id);
        public Response<Database.HomePage> Save(HomePageModelEdit data);
        public Response Delete(List<int> ids);

        public Response<string> GenerateHomePageHtml(long pid);

    }
}