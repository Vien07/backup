using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.LayoutPage.Models;
using X.PagedList;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Services
{
    public interface IQuickToolBarService
    {
         public Response<IPagedList<Database.QuickToolBar>> GetList(ParamSearch search);
        public Response<Database.QuickToolBar> GetById(int id);
        public Response<Database.QuickToolBarItem> GetChildById(int id);
        public Response<Database.QuickToolBar> Save(QuickToolBarModelEdit data );
        public Response<Database.QuickToolBarItem> SaveChild(QuickToolBarItemModelEdit data );
        public Response DeleteChild(List<int> ids);
        public Response Delete(List<int> ids);
        public Response MoveChild(int fromId, int toId);

        public Response<List<Database.QuickToolBarItem>> GetChildList(long? Pid);
        public Response<string> GenerateQuickToolBarHtml(long pid);

    }
}