using Admin.TemplatePage;
using Admin.TemplatePage.Models;
using Admin.TemplatePage.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.TemplatePage.Controllers
{


    public partial class TemplatePageController : Controller
    {
        public ITemplatePageService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        private readonly IRepository<Database.TemplatePage> _repoTemplatePage;
        private readonly IRepositoryConfig<Database.TemplatePageConfig> _repTemplatePageConfig;
        public PageModel _pageModel = new PageModel();
        public TemplatePageController(
            ITemplatePageService srv, 
            IViewRendererHelper viewRender,
            IRepository<Database.TemplatePage> repoTemplatePage,
            IRepositoryConfig<Database.TemplatePageConfig> repTemplatePageConfig,
            ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _srv = srv;
            _repoTemplatePage = repoTemplatePage;
            _repTemplatePageConfig = repTemplatePageConfig;
            _logger = logger;
        }
    }

}
