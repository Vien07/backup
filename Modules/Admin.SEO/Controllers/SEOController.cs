//using Admin.SEO.Models;
using Admin.SEO;
using Admin.SEO.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Base;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using X.PagedList;
using Admin.SEO.Services;
namespace Admin.SEO.Controllers
{


    public partial class SEOController : Controller
    {

        public ISEOService _srv;

        private readonly IRepository<Database.SEO> _repSEO;
        private readonly IRepository<Database.SEO_Files> _repSEO_Files;
        private readonly IRepositoryConfig<Database.SEOConfig> _repSEOConfig;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        IMetaHelper _metaHelper;
        public UserModel CurrentUser;
        Dictionary<string, string> _CONFIG;

        public PageModel _pageModel = new PageModel();
        public SEOController(
            ISEOService srv,
            IIdentityService indentitySrv,
            IMetaHelper metaHelper,
            IRepository<Database.SEO> _repSEO,
            IRepositoryConfig<Database.SEOConfig> repSEOConfig,
            IRepository<Database.SEO_Files> repSEO_Files,
            IViewRendererHelper viewRender, ILoggerHelper logger)
        {
           
            _viewRender = viewRender;
            _logger = logger;
            _repSEO = _repSEO;
            _repSEO_Files = repSEO_Files;
            _repSEOConfig = repSEOConfig;
            _CONFIG = _repSEOConfig.GetAllConfigs();
            _srv = srv;
            CurrentUser = indentitySrv.GetUser();
            CreateUser = CurrentUser.UserName;
        }
    }

}
