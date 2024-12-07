//using Admin.EmailManagement.Models;
using Admin.EmailManagement;
using Admin.EmailManagement.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using X.PagedList;
using Admin.EmailManagement.Servcies;
using Admin.EmailManagement.Services;
namespace Admin.EmailManagement.Controllers
{
    #region Define
    public class PageModel
    {
        public PageTitleModel PageTitle = new PageTitleModel("Thông tin cấu hình email", "Danh sách", "fas fa-layer-group", "/EmailManagement");

        public IPagedList<Database.EmailAdmin> List;
        public Dictionary<string,string> Configs;
        public Database.EmailAdmin EditModel;
        public ParamSearch Search;
        public PaginationModel Pagination = new PaginationModel();
    }
    #endregion

    public partial class EmailAdminController : Controller
    {
        public IEmailAdminService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";
        private readonly IRepositoryConfig<Database.EmailConfig> _repConfig;
        private readonly IRepository<Database.EmailAdmin> _repEmailAdmin;
        public PageModel _pageModel = new PageModel();
        public EmailAdminController(
            IEmailAdminService srv,
            IRepositoryConfig<Database.EmailConfig> repConfig,
             IRepository<Database.EmailAdmin> repEmailAdmin,
            IViewRendererHelper viewRender,
            ILoggerHelper logger)
        {
            _repConfig = repConfig;
            _repEmailAdmin = repEmailAdmin;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
        }
    }

}
