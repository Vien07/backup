//using Admin.EmailManagement.Models;
using Admin.EmailManagement;
using Admin.EmailManagement.Models;
using Admin.EmailManagement.Servcies;
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
    public class PageEmailMailBoxModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Hòm thư", "Danh sách", "fas fa-layer-group", "/EmailManagement");

        public IPagedList<Database.EmailMailBox> List;
        public Dictionary<string,string> Configs;
        public Database.EmailMailBox EditModel;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();
    }
    #endregion

    public partial class EmailMailBoxController : Controller
    {
        public IEmailMailBoxService _srv;
        public IEmailAdminService _srvAdmin;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        private readonly IRepositoryConfig<Database.EmailConfig> _repConfig;
        private readonly IRepository<Database.EmailMailBox> _repEmailMailBox;


        public PageEmailMailBoxModel _pageModel = new PageEmailMailBoxModel();
        public EmailMailBoxController(
            IRepositoryConfig<Database.EmailConfig> repConfig,
            IRepository<Database.EmailMailBox> repEmailMailBox,
            IEmailMailBoxService srv,
            IEmailAdminService srvAdmin,
            IViewRendererHelper viewRender,
            ILoggerHelper logger)
        {
            _repConfig = repConfig;
            _repEmailMailBox = repEmailMailBox;
            _viewRender = viewRender;
            _srv = srv;
            _srvAdmin = srvAdmin;
            _logger = logger;
        }
    }

}
