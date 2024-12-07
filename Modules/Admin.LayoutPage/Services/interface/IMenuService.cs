using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.LayoutPage.Models;
using X.PagedList;
using ComponentUILibrary.Models;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Services
{
    public interface IMenuService
    {
        //public Response<List<MultiLevelTreeList<Database.LayoutPage>>> GetList(ParamSearch search);
        public Response<List<Database.Menu>> GetList(ParamSearch search);
        public Response<LayoutPage.Database.Menu> GetById(int id);
        public Response<LayoutPage.Database.Menu> Save(MenuModelEdit data);
        public Response Delete(int id);

        public Response<List<SelectControlData>> GetMenuParent(int id);
        public Response<List<SelectControlData>> GetListMenuStyle();
        public Response<List<SelectControlData>> GetAllMenuParent();
        public Response UpdateParent(int id, int parentId);

        //public Response<List<SelectControlData>> GetMenuParent();

    }
}