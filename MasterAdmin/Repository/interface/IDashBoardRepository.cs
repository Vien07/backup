using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Admin.PostsManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.SEO.Models;
using Admin.PostsManagement.Api.Models;
using static MasterAdmin.Models.ApiWebsiteModel;
using Admin.Authorization.Database;
using MasterAdmin.Models.DashboardViewModel;
using Admin.DashBoard.Database;
using MasterAdmin.Models.Dashboard_ShortcutViewModel;

namespace MasterAdmin.Repository
{
    public interface IDashBoardRepository
    {
        public int GetNumberOfPost();
        public List<SiteMapModel> GetListPostCategories();
        public List<SiteMapModel> GetListParentPostCategories();

        public Dictionary<string, string> GetWebSiteConfig();
        public List<LogManagement> GetLogs(int takeItem);
        public List<Product_Item> GetListHotProduct(int takeItem);
        public List<Dashboard_Shortcut> GetShortcuts(int limit);
        public Response<Dashboard_Shortcut> SaveShortcuts(ShortCutSaveModel data);
        public bool Delete(int id);
        public Dashboard_Shortcut GetShortCutById(long id);
    }
}