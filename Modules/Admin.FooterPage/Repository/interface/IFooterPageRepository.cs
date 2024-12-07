using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.FooterPage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.FooterPage
{
    public interface IFooterPageRepository
    {
         public Response<IPagedList<Database.FooterPage>> GetList(ParamSearch search);
        public Response<Database.FooterPage> GetById(int id);
        public Response<Database.FooterItem> GetChildById(int id);
        public Response<FooterPage.Database.FooterPage> Save(FooterPageModelEdit data );
        public Response<FooterPage.Database.FooterItem> SaveChild(FooterItemModelEdit data );
        public Response DeleteChild(List<int> ids);
        public Response Delete(List<int> ids);
        public Response Enable(List<int> ids, bool isEnable);
        public Response Move(int fromId, int toId);
        public Response EnableUpdateOrder();
        public Response UpdateOrder(int id,double order);
        public Response<List<FooterPage.Database.FooterPageConfig>> SaveConfig(IFormCollection formData,string tab);
        public Response<List<FooterPage.Database.FooterPageConfig>> GetAllConfigs();
        public Response<FooterPage.Database.FooterPageConfig> GetConfigByKey(string key);
        public Response<List<Database.FooterItem>> GetChildList(long? Pid);
    }
}