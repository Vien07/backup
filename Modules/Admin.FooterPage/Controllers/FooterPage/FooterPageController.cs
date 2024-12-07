//using Admin.FooterPage.Models;
using Admin.FooterPage;
using Admin.FooterPage.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.FooterPage
{


    public partial class FooterPageController : Controller
    {
        public IFooterPageRepository _rep;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public FooterPageModel _pageModel = new FooterPageModel();
        public FooterPageController(IFooterPageRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _logger = logger;
        }
    }

}
