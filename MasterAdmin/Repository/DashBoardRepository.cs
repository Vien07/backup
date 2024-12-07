using Admin.PostsManagement.Database;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Base.Constant;
using Admin.SEO.Database;
using Admin.PostsCategory.Database;
using static MasterAdmin.Models.ApiWebsiteModel;
using Admin.PostsCategory.Constants;
using Admin.Course.Database;
using Admin.WebSetting.Constants;
using Admin.Authorization.Database;
using Admin.ProductManagement.Database;
using MasterAdmin.Models.DashboardViewModel;
using MasterAdmin.Models.Dashboard_ShortcutViewModel;
using Admin.ProductManagement.Constants;
using Admin.DashBoard.Database;
using Steam.Core.Base;
using Admin.Collection.Database;
using Admin.PostsCategory;
using Admin.PostsManagement.Services;
using Admin.SEO.Services;
using Steam.Infrastructure.Repository;
using Admin.PostsCategory.Services;
namespace MasterAdmin.Repository
{
    public class DashBoardRepository : IDashBoardRepository
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

        PostsManagementService _srvPostsManagementService;
        ISEOService _srvSEO;

        IPostsCategoryService _srvPostsCategoryService;
        ProductManagementContext _dbProduct;
        CollectionContext _dbCollection;
        WebsiteConfigurationContext _dbWebsiteConfiguration;
        AuthorizationContext _dbAuth;
        DashBoardContext _dbDashboard;
        readonly UserModel CurrentUser;
        readonly IIdentityService _identitySrv;
        string DOMAIN;
        public DashBoardRepository(
            IRepository<PostsManagement> repPostsManagement,
             IRepositoryConfig<PostsCategoryConfig> repPostsConfig,
           IRepository<PostsCategory> repPostsCategory,
            WebsiteConfigurationContext dbWebsiteConfiguration,
            IRepository<SEO> repSEO, 
            AuthorizationContext dbAuth,
            ProductManagementContext dbProduct, 
            ILoggerHelper logger,
            IIdentityService identitySrv,
             IPostsCategoryService srvPostsCategoryService,
            DashBoardContext dbDashboard)
        {
            _srvPostsCategoryService = srvPostsCategoryService;
            _repPostsManagement = repPostsManagement;
            _repPostsCategory = repPostsCategory;
            _repSEO = repSEO;
            _repPostsConfig = repPostsConfig;
            //_repPostsCateConfig = _repPostsCateConfig;
            _dbDashboard = dbDashboard;
            _dbWebsiteConfiguration = dbWebsiteConfiguration;
            _logger = logger;
            _dbAuth = dbAuth;
            _dbProduct = dbProduct;
            _CONFIG = _repPostsConfig.GetAllConfigs();
            //_CONFIGCATEPOST = _repPostsCateConfig.GetAllConfigs();
            _CONFIGWEBSITE = _dbWebsiteConfiguration.Configurations.Select(p => new { p.Key, p.Value }).ToDictionary(p => p.Key, p => p.Value);
            DOMAIN = _CONFIGWEBSITE[WebSettingConstants.ConfigName.RootDomain];
            _identitySrv = identitySrv;
            CurrentUser = _identitySrv.GetUser();
        }
        public Dictionary<string, string> GetWebSiteConfig()
        {
            try
            {
                return _CONFIGWEBSITE;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetNumberOfPost()
        {
            var number = 0;
            try
            {
                var listSeo = _repSEO.Query().Where(p => p.ModuleCode == "PostsManagement").ToList();
                number = listSeo.Count();
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return number;

        }
        public List<SiteMapModel> GetListPostCategories()
        {
            List<SiteMapModel> rs = new List<SiteMapModel>();
            try
            {
                var preslug = _CONFIGCATEPOST[PostsCategoryConstants.Config.Website.PreSlug]; //danh-muc
                var listPost = _repPostsManagement.Query().Where(p => p.Deleted == false && p.Enabled == true)
                     .OrderBy(p => p.Order).ToList();
                var listParrentCate = _repPostsCategory.Query().Where(p => p.ParentID == 0).ToList();
                var listChildCate = _repPostsCategory.Query().Where(p => p.ParentID != 0).ToList();
                var cate = (
                  from a in listChildCate
                  join b in listParrentCate on a.RootParentID equals b.Pid
                  select new SiteMapModel
                  {

                      loc = string.Format("{0}/{1}/{2}/{3}", DOMAIN, preslug, a.Slug, b.Slug),
                      lastmod = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK"),

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
        public List<SiteMapModel> GetListParentPostCategories()
        {
            List<SiteMapModel> rs = new List<SiteMapModel>();
            try
            {
                var preslug = _CONFIGCATEPOST[PostsCategoryConstants.Config.Website.PreSlug]; //danh-muc
                var listPost = _repPostsManagement.Query().Where(p => p.Deleted == false && p.Enabled == true)
                     .OrderBy(p => p.Order).ToList();
                var listParrentCate = _repPostsCategory.Query().Where(p => p.ParentID == 0).ToList();
                var cate = (
                  from a in listParrentCate
                  select new SiteMapModel
                  {

                      loc = string.Format("{0}/{1}/{2}/{3}", DOMAIN, preslug, a.Slug),
                      lastmod = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK"),

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
        public List<LogManagement> GetLogs(int takeItem)
        {
            List<LogManagement> logs = new List<LogManagement>();
            try
            {
                logs = _dbAuth.LogManagements.OrderByDescending(p => p.UpdateDate).Take(takeItem).ToList();
            }
            catch (Exception ex)
            {

            }
            return logs;
        }
        public List<Product_Item> GetListHotProduct(int takeItem)
        {
            List<Product_Item> listProductHot = new List<Product_Item>();

            try
            {

                var listProducts = _dbProduct.Products.Where(p => p.Deleted == false).ToList();

                var listCate = _dbProduct.ProductCategories.ToList();
                var listSeos = _repSEO.Query().Where(p => p.ModuleCode == ProductConstants.ModuleInfo.ModuleCode).OrderByDescending(p => p.CountView).Take(takeItem).ToList();

                listProductHot = (
                 from a in listSeos
                 join b in listProducts on a.PostPid equals b.Pid
                 join c in listCate on b.CateID equals c.Pid into cateGroup
                 from c in cateGroup.DefaultIfEmpty() //LEFT OUTER JOIN
                 select new Product_Item
                 {
                     Pid = a.Pid,
                     Slug = a != null ? a.PostSlug : null, // Handle NULL values from outer join
                     Description = a.Description,
                     Images = a.Images,
                     Enabled = a.Enabled,
                     Order = a.Order,
                     CountView = a.CountView,
                     Cate = c != null ? c.Title : null, // Handle NULL values from outer join
                     CateSlug = c != null ? c.Slug : "", // Handle NULL values from outer join
                     FilePath = b.FilePath,
                     ImagePath = SystemInfo.MedidaFileServer + b.FilePath + b.Images,
                 })

                 .ToList();






            }
            catch (Exception ex)
            {

            }
            return listProductHot;

        }
        public List<Dashboard_Shortcut> GetShortcuts(int limit)
        {
            List<Dashboard_Shortcut> shortcuts = new List<Dashboard_Shortcut>();
            try
            {
                shortcuts = _dbDashboard.Dashboard_Shortcuts.OrderByDescending(p => p.Order).Take(limit).ToList();
            }
            catch (Exception ex)
            {

            }
            return shortcuts;
        }

        public Dashboard_Shortcut GetShortCutById(long id)
        {
            Dashboard_Shortcut shortcut = new Dashboard_Shortcut();
            try
            {
                shortcut = _dbDashboard.Dashboard_Shortcuts.Where(p => p.Pid == id).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return shortcut;
        }
  

        public Response<Dashboard_Shortcut> SaveShortcuts(ShortCutSaveModel data)
        {

            #region Validate
            var validator = new ShortcutValidator();

            ValidationResult results = validator.Validate(data);
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<Dashboard_Shortcut> rs = new Response<Dashboard_Shortcut>();
            Dashboard_Shortcut saveData = new Dashboard_Shortcut();
            if (!success)
            {
                string mess = string.Join(";", results.Errors);

                rs.Message = mess;
                rs.IsError = true;
                return rs;
            }
            #endregion
            using (var transaction = _dbDashboard.Database.BeginTransaction())
            {
                try
                {
                    saveData = data.GetDatabaseModel();

                    if (saveData.Pid == 0)
                    {
                        saveData.Order = 0.9;
                        saveData.CreateUser = CurrentUser.UserName;
                        saveData.UpdateUser = CurrentUser.UserName;
                        //modelResponse.FilePath = filePath;

                        _dbDashboard.Dashboard_Shortcuts.Add(saveData);

                        _dbDashboard.SaveChanges();


                    }
                    else
                    {
                        var editData = data.GetDatabaseModel();
                        saveData = _dbDashboard.Dashboard_Shortcuts.Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (saveData != null)
                        {


                            saveData.Name = editData.Name;
                            saveData.IconSvg = editData.IconSvg;
                            saveData.Link = editData.Link;
                            saveData.Order = editData.Order;
                            saveData.Description = editData.Description;
                            saveData.Enabled = editData.Enabled;
                            saveData.UpdateUser = CurrentUser.UserName;
                            saveData.UpdateDate = DateTime.Now;



                            _dbDashboard.SaveChanges();

                        }


                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    rs.IsError = true;
                    rs.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());

                }
            }
            rs.Data = saveData;
            return rs;

        }
        public bool Delete(int id)
        {


            try
            {


                var model = _dbDashboard.Dashboard_Shortcuts.Where(p => p.Pid == id).FirstOrDefault();
                _dbDashboard.Dashboard_Shortcuts.Remove(model);


                _dbDashboard.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToJson());
                return false;
            }
        }
    }

}
