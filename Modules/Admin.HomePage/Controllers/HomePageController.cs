using Admin.HomePage;
using Admin.HomePage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.HomePage
{


    public partial class HomePageController : Controller
    {
        public IHomePageRepository _rep;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public PageModel _pageModel = new PageModel();
        public HomePageController(IHomePageRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _logger = logger;
        }
    }

}
