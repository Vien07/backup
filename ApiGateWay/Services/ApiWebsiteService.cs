
using Microsoft.AspNetCore.Http;
using Admin.PostsManagement.Database;
using Admin.PostsManagement.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Utilities.SteamModels;
using Admin.PostsManagement.Constants;
using Admin.PostsManagement.Models;
using System.Drawing.Imaging;
using System.Xml.Linq;
using System.Text.Json;
using Steam.Core.Base.Constant;
using Admin.SEO.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Admin.SEO.Models;
using Admin.PostsManagement.Api.Models;
using Admin.PostsCategory.Database;
using ComponentUILibrary.Models;
using System.Dynamic;
using Admin.SEO.Api.Models;
using System.Net.WebSockets;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
//using Steam.Core.Common.TModel;
using System.Linq;
using Microsoft.AspNetCore.Components.Forms;
using ApiGateWay;
using static ApiGateWay.Models.ApiWebsiteModel;
using Admin.PostsCategory.Constants;
using Admin.ProductManagement.Database;
using Admin.ProductManagement.Constants;
using Admin.Collection.Database;
using Admin.Collection.Constants;
using Admin.Course.Database;
using Admin.WebSetting.Constants;
using Steam.Infrastructure.Repository;
using Admin.WebSetting.Database;
using System.Configuration;

namespace ApiGateWay.Services
{
    public class ApiWebsiteService : IApiWebsiteService
    {
        private ILoggerHelper _logger;
        Dictionary<string, string> _config;
        Dictionary<string, string> _configCatePost;
        Dictionary<string, string> _configWebsite;
        private readonly IRepository<PostsManagement> _repPostsManagement;
        private readonly IRepository<PostsCategory> _repPostsCategory;
        private readonly IRepository<SEO> _repSEO;
        private readonly IRepository<WebsiteConfiguration> _repWebsiteConfiguration;

        private readonly IRepositoryConfig<PostsCategoryConfig> _repPostsCategoryConfig;
        private readonly IRepositoryConfig<PostsManagementConfig> _repPostsManagementConfig;
        private readonly IRepositoryConfig<WebsiteConfiguration> _repConfiguration;
        string DOMAIN;
        public ApiWebsiteService(
           IRepository<PostsManagement> repPostsManagement,
            IRepository<PostsCategory> repPostsCategory,
           IRepository<WebsiteConfiguration> repWebsiteConfiguration,
            IRepository<SEO> repSEO,
            IRepositoryConfig<PostsCategoryConfig> repPostsCategoryConfig,
            IRepositoryConfig<PostsManagementConfig> repPostsManagementConfig,
            IRepositoryConfig<WebsiteConfiguration> repConfiguration,
        ILoggerHelper logger)
        {
            _repConfiguration = repConfiguration;
            _repPostsManagement = repPostsManagement;
            _repPostsManagementConfig = repPostsManagementConfig;
            _repPostsCategory = repPostsCategory;
            _repWebsiteConfiguration = repWebsiteConfiguration;
            _repSEO = repSEO;
            _repPostsCategoryConfig = repPostsCategoryConfig;
            _logger = logger;

            _config = _repPostsManagementConfig.GetAllConfigs();
            _configCatePost = _repPostsCategoryConfig.GetAllConfigs();
            _configWebsite = _repConfiguration.GetAllConfigs();
            DOMAIN = _configWebsite[WebSettingConstants.ConfigName.RootDomain];
 
        }
        public List<SiteMapModel> GetListPost()
        {
            List<SiteMapModel> rs = new List<SiteMapModel>();
            try
            {
                var ogImage = _configWebsite[WebSettingConstants.ConfigName.ogImage];
                var listPost = _repPostsManagement.Query().Where(p => p.Deleted == false && p.Enabled == true)
                     .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();
                var listCate = _repPostsCategory.Query().Where(p=>p.Enabled==true).ToList();
                var listSeo = _repSEO.Query().Where(p=>p.ModuleCode== PostsManagementConstants.ModuleInfo.ModuleCode).ToList();
                var posts = (
                  from b in listSeo
                  join a in listPost on b.PostPid equals a.Pid  
                  join c in listCate on a.CateID equals c.Pid 
                  select new SiteMapModel
                  {

                      loc = string.Format("{0}/{1}/{2}", DOMAIN, c.Slug, b.PostSlug),
                      lastmod = b.UpdateDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"),
                      image_loc = string.Format("{0}", SystemInfo.MedidaFileServer + (!String.IsNullOrEmpty(a.Images) ?  a.FilePath + a.Images: WebSettingConstants.StaticPath.Asset.Image + ogImage)),
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
                var preslug = _configCatePost[PostsCategoryConstants.Config.Website.PreSlug]; //danh-muc
           
                var listParrentCate =_repPostsCategory.Query().Where(p=>p.Enabled==true).Where(p=>p.ParentID==0).ToList();
                var listChildCate = _repPostsCategory.Query().Where(p=>p.Enabled==true).Where(p=>p.ParentID != 0).ToList();
                var cate = (
                  from a in listParrentCate where !String.IsNullOrEmpty(a.WebsiteCatePage)
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
                if(cate !=null)
                {
                     rs.AddRange(cate);

                }  
                if(cateChild != null)
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
        //        var ogImage = _configWebsite[WebSettingConstants.ConfigName.ogImage];

        //        var preslug = _dbProduct.ProductCategoryConfigs.Where(p => p.Key == ProductCategoryConstants.Config.Website.PreSlugDetail).FirstOrDefault().Value;

        //        var listProduct = _dbProduct.Products.Where(p => p.Deleted == false && p.Enabled == true)
        //             .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList();
        //        var listCate = _dbProduct.ProductCategories.Where(p => p.Enabled == true).ToList();
        //        var listSeo = _dbSEO.SEOs.Where(p => p.ModuleCode == ProductConstants.ModuleInfo.ModuleCode).ToList();
        //        var products = (
        //          from b in listSeo
        //          join a in listProduct on b.PostPid equals a.Pid
        //          select new SiteMapModel
        //          {

        //              loc = string.Format("{0}/{1}/{2}", DOMAIN, preslug, b.PostSlug),
        //              lastmod = b.UpdateDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"),
        //              image_loc = string.Format("{0}", SystemInfo.MedidaFileServer + (!String.IsNullOrEmpty(a.Images) ? a.FilePath + a.Images : WebSettingConstants.StaticPath.Asset.Image + ogImage)),
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

        //public List<SiteMapModel> GetListProductCategories()
        //{
        //    List<SiteMapModel> rs = new List<SiteMapModel>();
        //    try
        //    {
        //        var preslug = _dbProduct.ProductCategoryConfigs.Where(p => p.Key == ProductCategoryConstants.Config.Website.PreSlugCate).FirstOrDefault().Value;

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
                var preslug = _configCatePost[PostsCategoryConstants.Config.Website.PreSlug]; //danh-muc
                var listPost = _repPostsManagement.Query().Where(p => p.Deleted == false && p.Enabled == true)
                     .OrderBy(p => p.Order).ToList();
                var listParrentCate = _repPostsCategory.Query().Where(p => p.ParentID == 0).ToList();
                var cate = (
                  from a in listParrentCate
                  select new SiteMapModel
                  {

                      loc = string.Format("{0}/{1}/{2}/{3}", DOMAIN, preslug, a.Slug),
                      lastmod =a.UpdateDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'sszzz"),

                  })
                  .ToList();
                rs = cate;
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message,"");

            }
            return rs;
        }
    }

}
