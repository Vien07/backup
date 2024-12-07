using Admin.PostsManagement.Database;
using System.Reflection;
using X.PagedList;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Common.SteamString;
using Admin.PostsManagement.Constants;
using Admin.SEO.Database;
using Admin.PostsCategory.Database;
using static MasterAdmin.Models.ApiWebsiteModel;
using Admin.PostsCategory.Constants;
using Admin.Course.Database;
using Admin.WebSetting.Constants;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.Constants;
using Admin.Collection.Database;
using Admin.Collection.Constants;
using Admin.SEO.Services;
using Admin.PostsManagement.Services;
using Steam.Infrastructure.Repository;
using Admin.PostsCategory;
using Admin.PostsCategory.Services;

namespace MasterAdmin.Repository
{
    public class ApiWebsiteRepository : IApiWebsiteRepository
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _CONFIG;
        Dictionary<string, string> _CONFIGCATEPOST;
        Dictionary<string, string> _CONFIGWEBSITE;

        private readonly IRepository<PostsManagement> _repPostsManagement;
        private readonly IRepository<PostsCategory> _repPostsCategory;
        private readonly IRepository<SEO> _repSEO;

        private readonly IRepositoryConfig<PostsCategoryConfig> _repPostsConfig;
        //private readonly IRepositoryConfig<PostsManagementCateConfig> _repPostsCateConfig;

        IPostsManagementService _srvPostsManagementService;
        ISEOService _srvSEO;

        IPostsCategoryService _srvPostsCategoryService;
        WebsiteConfigurationContext _dbWebsiteConfiguration;
        string DOMAIN;
        public ApiWebsiteRepository(
            IRepositoryConfig<PostsCategoryConfig> repPostsConfig,
            IRepository<Admin.SEO.Database.SEO> repSEO,
            IRepository<PostsCategory> repPostsCategory,
            IRepository<PostsManagement> repPostsManagement,
            IPostsManagementService srvPostsManagementService,
            CollectionContext dbCollection,
            IPostsCategoryService srvPostsCategoryService,
            ProductManagementContext dbProduct,
            WebsiteConfigurationContext dbWebsiteConfiguration,
            ISEOService srvSEO,
            ILoggerHelper logger)
        {
            _repSEO = repSEO;
            _repPostsCategory = repPostsCategory;
            _repPostsManagement = repPostsManagement;
            _srvPostsManagementService = srvPostsManagementService;
            _repPostsConfig = repPostsConfig;
            //_repPostsCateConfig = repPostsCateConfig;
            _srvSEO = srvSEO;
            _dbWebsiteConfiguration = dbWebsiteConfiguration;
            _srvPostsCategoryService = srvPostsCategoryService;
            _logger = logger;

            _CONFIG = _repPostsConfig.GetAllConfigs();
            //_CONFIGCATEPOST = _repPostsCateConfig.GetAllConfigs();
            _CONFIGWEBSITE = _dbWebsiteConfiguration.Configurations.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);
            DOMAIN = _CONFIGWEBSITE[WebSettingConstants.ConfigName.RootDomain];

        }
        public List<SiteMapModel> GetListPost()
        {
            List<SiteMapModel> rs = new List<SiteMapModel>();
            try
            {
                var listPost = _repPostsManagement.Query().Where(p => p.Deleted == false && p.Enabled == true)
                     .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();
                var listCate = _repPostsCategory.Query().Where(p => p.Enabled == true).ToList();
                var listSeo = _repSEO.Query().Where(p => p.ModuleCode == PostsManagementConstants.ModuleInfo.ModuleCode).ToList();
                var posts = (
                  from b in listSeo
                  join a in listPost on b.PostPid equals a.Pid
                  join c in listCate on a.CateID equals c.Pid
                  select new SiteMapModel
                  {

                      loc = string.Format("{0}/{1}/{2}", DOMAIN, c.Slug, b.PostSlug),
                      lastmod = b.UpdateDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"),
                      image_loc = string.Format("{0}{1}", DOMAIN, (a.FilePath + a.Images).CheckExistsImage()),
                      image_title = a.Images_Alt,
                      image_caption = a.Images_Caption

                  })
                  .ToList();
                rs = posts;
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }

        public List<SiteMapModel> GetListPostCategories()
        {
            List<SiteMapModel> rs = new List<SiteMapModel>();
            try
            {
                var preslug = _CONFIGCATEPOST[PostsCategoryConstants.Config.Website.PreSlug]; //danh-muc

                var listParrentCate = _repPostsCategory.Query().Where(p => p.Enabled == true).Where(p => p.ParentID == 0).ToList();
                var listChildCate = _repPostsCategory.Query().Where(p => p.Enabled == true).Where(p => p.ParentID != 0).ToList();
                var cate = (
                  from a in listParrentCate
                  where !String.IsNullOrEmpty(a.WebsiteCatePage)
                  select new SiteMapModel
                  {

                      loc = string.Format("{0}/{1}/{2}/", DOMAIN, preslug, a.Slug),
                      lastmod = a.UpdateDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"),

                  })
                  .ToList();
                var cateChild = (
              from a in listParrentCate
              join b in listChildCate on a.Pid equals b.RootParentID
              select new SiteMapModel
              {

                  loc = string.Format("{0}/{1}/{2}/{3}", DOMAIN, preslug, a.Slug, b.Slug),
                  lastmod = b.UpdateDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"),

              })
              .ToList();
                if (cate != null)
                {
                    rs.AddRange(cate);

                }
                if (cateChild != null)
                {
                    rs.AddRange(cateChild);

                }
            }
            catch (Exception ex)
            {
                rs = new List<SiteMapModel>();
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
        //public List<SiteMapModel> ProductsSitemap()
        //{
        //    List<SiteMapModel> rs = new List<SiteMapModel>();
        //    try
        //    {
        //        var listProduct = _dbProduct.Products.Where(p => p.Deleted == false && p.Enabled == true)
        //             .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();
        //        var listCate = _dbProduct.ProductCategories.Where(p => p.Enabled == true).ToList();
        //        var listSeo = _repSEO.Query().Where(p => p.ModuleCode == ProductConstants.ModuleInfo.ModuleCode).ToList();
        //        var products = (
        //          from b in listSeo
        //          join a in listProduct on b.PostPid equals a.Pid
        //          join c in listCate on a.CateID equals c.Pid
        //          select new SiteMapModel
        //          {

        //              loc = string.Format("{0}/{1}/{2}", DOMAIN, c.Slug, b.PostSlug),
        //              lastmod = b.UpdateDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"),
        //              image_loc = string.Format("{0}{1}", DOMAIN, (a.FilePath + a.Images).CheckExistsImage()),
        //              image_title = a.Images_Alt,
        //              image_caption = a.Images_Caption

        //          })
        //          .ToList();
        //        rs = products;
        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

        //    }
        //    return rs;
        //}
        //public List<SiteMapModel> CollectionSitemap()
        //{
        //    List<SiteMapModel> rs = new List<SiteMapModel>();
        //    try
        //    {
        //        var listCollection = _dbCollection.Collections.Where(p => p.Deleted == false && p.Enabled == true)
        //             .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();
        //        var listCate = _repPostsCategory.Query().Where(p => p.Enabled == true).ToList();
        //        var listSeo = _repSEO.Query().Where(p => p.ModuleCode == CollectionConstants.ModuleInfo.ModuleCode).ToList();
        //        var collections = (
        //          from b in listSeo
        //          join a in listCollection on b.PostPid equals a.Pid
        //          join c in listCate on a.CateID equals c.Pid
        //          select new SiteMapModel
        //          {

        //              loc = string.Format("{0}/{1}/{2}", DOMAIN, c.Slug, b.PostSlug),
        //              lastmod = b.UpdateDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"),
        //              image_loc = string.Format("{0}{1}", DOMAIN, (a.FilePath + a.Images).CheckExistsImage()),
        //              image_title = a.Images_Alt,
        //              image_caption = a.Images_Caption

        //          })
        //          .ToList();
        //        rs = collections;
        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

        //    }
        //    return rs;
        //}

        //public List<SiteMapModel> GetListProductCategories()
        //{
        //    List<SiteMapModel> rs = new List<SiteMapModel>();
        //    try
        //    {
        //        var preslug = _CONFIGCATEPOST[PostsCategoryConstants.Config.Website.PreSlug]; //danh-muc

        //        var listParrentCate = _dbProduct.ProductCategories.Where(p => p.Enabled == true).ToList();
        //        var cate = (
        //          from a in listParrentCate
        //          select new SiteMapModel
        //          {

        //              loc = string.Format("{0}/{1}/{2}/", DOMAIN, preslug, a.Slug),
        //              lastmod = a.UpdateDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"),

        //          })
        //          .ToList();

        //        if (cate != null)
        //        {
        //            rs.AddRange(cate);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        rs = new List<SiteMapModel>();
        //        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

        //    }
        //    return rs;
        //}
        public List<SiteMapModel> GetListParentPostCategories()
        {
            List<SiteMapModel> rs = new List<SiteMapModel>();
            try
            {
                var preslug = _CONFIGCATEPOST[PostsCategoryConstants.Config.Website.PreSlug]; //danh-muc
                var listPost = _repPostsCategory.Query().Where(p => p.Deleted == false && p.Enabled == true)
                     .OrderBy(p => p.Order).ToList();
                var listParrentCate = _repPostsCategory.Query().Where(p => p.ParentID == 0).ToList();
                var cate = (
                  from a in listParrentCate
                  select new SiteMapModel
                  {

                      loc = string.Format("{0}/{1}/{2}/{3}", DOMAIN, preslug, a.Slug),
                      lastmod = a.UpdateDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"),

                  })
                  .ToList();
                rs = cate;
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }
    }

}
