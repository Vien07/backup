//using Admin.LayoutPage.Models;
using Admin.LayoutPage;
using Admin.LayoutPage.Models;
using Admin.LayoutPage.Repository;
using Admin.LayoutPage.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.LayoutPage.Controllers
{

    public partial class MenuController : Controller
    {
        IMenuService _srv;
        IMenuStyleService _srvMenuStyle;
        private readonly IRepositoryConfig<Database.MenuConfig> _repConfig;
        private readonly IRepositoryConfig<Database.MenuStyleConfig> _repMenuStyleConfig;
        private readonly IRepository<Database.Menu> _repMenu;
        private readonly IRepository<Database.MenuStyle> _repMenuStyle;

        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public MenuController(
             IRepositoryConfig<Database.MenuStyleConfig> repMenuStyleConfig,
           IMenuService srv,
           IMenuStyleService srvMenuStyle,
            IViewRendererHelper viewRender,
             IRepositoryConfig<Database.MenuConfig> repConfig,
            ILoggerHelper logger)
        {
            _repMenuStyleConfig = repMenuStyleConfig;
            _repConfig = repConfig;
            _viewRender = viewRender;
            _srv = srv;
            _srvMenuStyle = srvMenuStyle;
            _logger = logger;
        }
    }

}
