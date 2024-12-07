using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.HeaderPage.Models;
using X.PagedList;
using ComponentUILibrary.Models;
using Steam.Core.Base.Models;

namespace Admin.HeaderPage
{
    public interface IMenuRepository
    {
        //public Response<List<MultiLevelTreeList<Database.HeaderPage>>> GetList(ParamSearch search);
        public Response<List<Database.Menu>> GetList(ParamSearch search);
        public Response<HeaderPage.Database.Menu> GetById(int id);
        public Response<HeaderPage.Database.Menu> Save(MenuModelEdit data);
        public Response Delete(int id);
        public Response Enable(List<int> ids, bool isEnable);
        public Response UpdateParent(int id, int parrentId);
        public Response Move(int fromId, int toId);
        public Response EnableUpdateOrder();
        public Response UpdateOrder(int id,double order);
        public Response<List<HeaderPage.Database.MenuConfig>> SaveConfig(IFormCollection formData,string tab);
        public Response<List<HeaderPage.Database.MenuConfig>> GetAllConfigs();
        public Response<HeaderPage.Database.MenuConfig> GetConfigByKey(string key);
        public Response<List<SelectControlData>> GetMenuParent(int id);
        public Response<List<SelectControlData>> GetListMenuStyle();
        //public Response<List<SelectControlData>> GetMenuParent();

    }
}