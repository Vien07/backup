using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.LayoutPage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Services
{
    public interface IMenuStyleService
    {
         public Response<IPagedList<Database.MenuStyle>> GetList(ParamSearch search);
        public Response<Database.MenuStyle> GetById(int id);
        public Response<Database.MenuItemStyle> GetChildById(int id);
        public Response<LayoutPage.Database.MenuStyle> Save(MenuStyleModelEdit data );
        public Response<LayoutPage.Database.MenuItemStyle> SaveChild(MenuItemStyleModelEdit data );
        public Response DeleteChild(List<int> ids);
        public Response Delete(List<int> ids);

        public Response<List<Database.MenuItemStyle>> GetChildList(long? Pid);
    }
}