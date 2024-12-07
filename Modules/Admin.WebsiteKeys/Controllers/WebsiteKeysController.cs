using Admin.WebsiteKeys;
using Admin.WebsiteKeys.Models;
using Admin.WebsiteKeys.Service;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.WebsiteKeys.Controllers
{


    public partial class WebsiteKeysController : Controller
    {
        private readonly IRepositoryConfig<Database.WebsiteKeysConfig> _repWebsiteKeysConfig;
        private readonly IRepository<Database.WebsiteKeys> _repoWebsiteKeys;
        public IWebsiteKeysService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public PageModel _pageModel = new PageModel();
        public WebsiteKeysController(
            IWebsiteKeysService srv,
            IRepositoryConfig<Database.WebsiteKeysConfig> repWebsiteKeysConfig,
             IRepository<Database.WebsiteKeys> repoWebsiteKeys,
            IViewRendererHelper viewRender,
            ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _repWebsiteKeysConfig = repWebsiteKeysConfig;
            _repoWebsiteKeys = repoWebsiteKeys;
            _srv = srv;
            _logger = logger;
        }
    }

}
