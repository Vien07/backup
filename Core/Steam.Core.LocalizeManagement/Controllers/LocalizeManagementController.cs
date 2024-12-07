using Steam.Core.LocalizeManagement;
using Steam.Core.LocalizeManagement.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using Steam.Core.LocalizeManagement.Services;
using Steam.Infrastructure.Repository;
namespace Steam.Core.LocalizeManagement.Controllers
{


    public partial class LocalizeManagementController : Controller
    {
        public ILocalizeManagementService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        private readonly IRepositoryConfig<Database.LocalizeManagementConfig> _repConfig;
        private readonly IRepository<Database.LocalizeManagement> _repLocalizeManagement;

        public PageModel _pageModel = new PageModel();
        public LocalizeManagementController(
             IRepository<Database.LocalizeManagement> repLocalizeManagement,
            IRepositoryConfig<Database.LocalizeManagementConfig> repConfig,
            ILocalizeManagementService srv, 
            IViewRendererHelper viewRender, 
            ILoggerHelper logger)
        {
            _repLocalizeManagement = repLocalizeManagement;
            _viewRender = viewRender;
            _srv = srv;
            _repConfig = repConfig;
            _logger = logger;
        }
    }

}
