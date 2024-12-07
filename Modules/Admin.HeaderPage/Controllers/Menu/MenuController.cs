//using Admin.HeaderPage.Models;
using Admin.HeaderPage;
using Admin.HeaderPage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.HeaderPage
{

    public partial class MenuController : Controller
    {
        public IMenuRepository _rep;
        public IMenuStyleRepository _menuStyleRep;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public MenuController(IMenuRepository rep, IMenuStyleRepository menuStyleRep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _menuStyleRep = menuStyleRep;
            _logger = logger;
        }
    }

}
