//using Admin.LayoutPage.Models;
using Admin.LayoutPage;
using Admin.LayoutPage.Models;
using Admin.LayoutPage.Repository;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;
using Admin.LayoutPage.Services;
using Steam.Infrastructure.Repository;
namespace Admin.LayoutPage.Controllers
{


    public partial class MenuStyleController : Controller
    {
        IMenuStyleService _srv;
        public ILoggerHelper _logger;

        private readonly IRepositoryConfig<Database.MenuStyleConfig> _repMenuStyleConfig;
        private readonly IRepository<Database.MenuStyle> _repMenuStyle;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public MenuStyleModel _pageModel = new MenuStyleModel();
        public MenuStyleController(
            IRepositoryConfig<Database.MenuStyleConfig> repMenuStyleConfig,
             IRepository<Database.MenuStyle> repMenuStyle,
            IMenuStyleService srv,
            IViewRendererHelper viewRender, 
            ILoggerHelper logger)
        {
            _repMenuStyleConfig = repMenuStyleConfig;
            _repMenuStyle = repMenuStyle;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
        }
    }

}
