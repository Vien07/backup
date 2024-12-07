//using Admin.EmailManagement.Models;
using Admin.EmailManagement;
using Admin.EmailManagement.Models;
using Admin.EmailManagement.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.EmailManagement.Controllers
{
    #region Define
    public class PageEmailTemplateModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Định nghĩa mẫu Email", "Danh sách", "fas fa-layer-group", "/EmailManagement");

        public IPagedList<Database.EmailTemplate> List;
        public Dictionary<string, string> Configs;
        public Database.EmailTemplate EditModel;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();
    }
    #endregion

    public partial class EmailTemplateController : Controller
    {
        public IEmailTemplateService _srv;
        public IEmailAdminService _srvAdmin;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        private readonly IRepositoryConfig<Database.EmailConfig> _repConfig;
        private readonly IRepository<Database.EmailTemplate> _repEmailTemplate;

        public string CreateUser = "admin";

        public PageEmailTemplateModel _pageModel = new PageEmailTemplateModel();
        public EmailTemplateController(
            IEmailTemplateService srv,
            IEmailAdminService srvAdmin,
            IViewRendererHelper viewRender,
             IRepositoryConfig<Database.EmailConfig> repConfig,
             IRepository<Database.EmailTemplate> repEmailTemplate,
            ILoggerHelper logger)
        {
            _repEmailTemplate = repEmailTemplate;
            _repConfig = repConfig;
            _viewRender = viewRender;
            _srv = srv;
            _srvAdmin = srvAdmin;
            _logger = logger;
        }
    }

}
