using Admin.Authorization;
using Admin.Authorization.Database;
using Admin.Authorization.Models;
using Admin.Authorization.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.Authorization.Controllers
{


    public partial class LogManagementController : Controller
    {
        public ILogManagementService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        private readonly IRepository<LogManagement> _repLogManagement;
        private readonly IRepositoryConfig<LogManagementConfig> _repConfig;
        public PageModel _pageModel = new PageModel();
        public LogManagementController(
            ILogManagementService srv, 
            IViewRendererHelper viewRender,
            IRepositoryConfig<LogManagementConfig> repConfig,
            IRepository<LogManagement> repLogManagement,
            ILoggerHelper logger)
        {
            _repConfig = repConfig;
            _repLogManagement = repLogManagement;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
        }
    }

}
