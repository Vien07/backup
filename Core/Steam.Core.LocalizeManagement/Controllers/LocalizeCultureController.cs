using Steam.Core.LocalizeManagement;
using Steam.Core.LocalizeManagement.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using Steam.Infrastructure.Repository;
using Steam.Core.LocalizeManagement.Services;
namespace Steam.Core.LocalizeManagement.Controllers
{


    public partial class LocalizeCultureController : Controller
    {
        public ILocalizeCultureService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        private readonly IRepositoryConfig<Database.LocalizeCultureConfig> _repConfig;
        private readonly IRepository<Database.LocalizeCulture> _repLocalizeCulture;
        public PageModel _pageModel = new PageModel();
        public LocalizeCultureController(
            IRepositoryConfig<Database.LocalizeCultureConfig> repConfig,
            IRepository<Database.LocalizeCulture> repLocalizeCulture,
            ILocalizeCultureService srv,
            IViewRendererHelper viewRender, 
            ILoggerHelper logger)
        {
            _repConfig = repConfig;
            _repLocalizeCulture = repLocalizeCulture;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
        }
    }

}
