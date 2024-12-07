
using Admin.PostsManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Base;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;


namespace Admin.PostsManagement.Controllers
{

    public partial class PostsManagementController : Controller
    {
        IHttpContextAccessor _httpContext;
        public IPostsManagementService _srv;

        private readonly IRepository<Database.PostsManagement> _repoPostsManagement;
        private readonly IRepositoryConfig<Database.PostsManagementConfig> _repConfig;
        private readonly IRepository<Database.PostsManagement_Files> _repoPostsManagementFile;

        Dictionary<string, string> _CONFIG;

        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "";
        public  UserModel CurrentUser;
        public PageModel _pageModel = new PageModel();
        public PostsManagementController(
            IPostsManagementService srv,
            IIdentityService indentitySrv,
            IViewRendererHelper viewRender, 
            ILoggerHelper logger,
            IRepository<Database.PostsManagement> repoPostsManagement,
            IHttpContextAccessor httpContext,
            IRepositoryConfig<Database.PostsManagementConfig> repConfig,
            IRepository<Database.PostsManagement_Files> repoPostsManagementFile)
        {
            _httpContext = httpContext;
            _viewRender = viewRender;
            _srv = srv;
            _repoPostsManagement = repoPostsManagement;
            _logger = logger;
            CurrentUser = indentitySrv.GetUser();
            CreateUser = CurrentUser.UserName;
            _repConfig = repConfig;
            _repoPostsManagementFile = repoPostsManagementFile;
            _CONFIG = _repConfig.GetAllConfigs();

        }

    }

}
