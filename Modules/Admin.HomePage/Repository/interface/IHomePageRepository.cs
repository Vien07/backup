using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.HomePage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.HomePage
{
    public interface IHomePageRepository
    {
         public Response<IPagedList<Database.HomePage>> GetList(HomePageModel.ParamSearch search);
        public Response<HomePageModel.HomePageDetail> GetById(int id);
        public Response<Database.HomePage> Save(HomePageModelEdit data);
        public Response Delete(List<int> ids);
        public Response Enable(List<int> ids, bool isEnable);
        public Response Move(int fromId, int toId);
        public Response EnableUpdateOrder();
        public Response UpdateOrder(int id,double order);
        public Response<List<HomePage.Database.HomePageConfig>> SaveConfig(IFormCollection formData,string tab);
        public Response<List<HomePage.Database.HomePageConfig>> GetAllConfigs();
        public Response<HomePage.Database.HomePageConfig> GetConfigByKey(string key);
        public Response<string> GetHomePageHTML();

    }
}