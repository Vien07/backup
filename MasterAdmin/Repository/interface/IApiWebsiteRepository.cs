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

namespace MasterAdmin.Repository
{
    public interface IApiWebsiteRepository
    {
        public List<SiteMapModel> GetListPost();
        public List<SiteMapModel> GetListPostCategories();
        //public List<SiteMapModel> ProductsSitemap();
        public List<SiteMapModel> GetListParentPostCategories();
        //public List<SiteMapModel> GetListProductCategories();
        //public List<SiteMapModel> CollectionSitemap();



    }
}