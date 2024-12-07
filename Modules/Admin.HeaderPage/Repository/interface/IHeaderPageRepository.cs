using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.HeaderPage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.HeaderPage
{
    public interface IHeaderPageRepository
    {
         public Response<IPagedList<Database.HeaderPage>> GetList(ParamSearch search);
        public Response<Database.HeaderPage> GetById(int id);
        public Response<HeaderPage.Database.HeaderPage> Save(HeaderPage.Database.HeaderPage data );
        public Response Delete(List<int> ids);
        public Response Enable(List<int> ids, bool isEnable);
        public Response Move(int fromId, int toId);
        public Response EnableUpdateOrder();
        public Response UpdateOrder(int id,double order);
        public Response<List<HeaderPage.Database.HeaderPageConfig>> SaveConfig(IFormCollection formData,string tab);
        public Response<List<HeaderPage.Database.HeaderPageConfig>> GetAllConfigs();
        public Response<HeaderPage.Database.HeaderPageConfig> GetConfigByKey(string key);
        public Response<string> GenerateHeaderHtml();

    }
}