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


    public partial class MenuStyleController : Controller
    {
        public IMenuStyleRepository _rep;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public MenuStyleModel _pageModel = new MenuStyleModel();
        public MenuStyleController(IMenuStyleRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _logger = logger;
        }
    }

}
