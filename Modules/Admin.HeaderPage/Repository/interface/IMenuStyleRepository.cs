using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.HeaderPage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.HeaderPage
{
    public interface IMenuStyleRepository
    {
         public Response<IPagedList<Database.MenuStyle>> GetList(ParamSearch search);
        public Response<Database.MenuStyle> GetById(int id);
        public Response<Database.MenuItemStyle> GetChildById(int id);
        public Response<HeaderPage.Database.MenuStyle> Save(MenuStyleModelEdit data );
        public Response<HeaderPage.Database.MenuItemStyle> SaveChild(MenuItemStyleModelEdit data );
        public Response DeleteChild(List<int> ids);
        public Response Delete(List<int> ids);
        public Response Enable(List<int> ids, bool isEnable);
        public Response Move(int fromId, int toId);
        public Response EnableUpdateOrder();
        public Response UpdateOrder(int id,double order);
        public Response<List<HeaderPage.Database.MenuStyleConfig>> SaveConfig(IFormCollection formData,string tab);
        public Response<List<HeaderPage.Database.MenuStyleConfig>> GetAllConfigs();
        public Response<HeaderPage.Database.MenuStyleConfig> GetConfigByKey(string key);
        public Response<List<Database.MenuItemStyle>> GetChildList(long? Pid);
    }
}