using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Admin.Authorization.Services;
using Admin.Authorization.Database;
using Steam.Infrastructure.Repository;

namespace Admin.Authorization.Controllers
{

    public partial class AccountController: Controller
    {

        IViewRendererHelper _viewRender;
        public IAccountService _srv;
        public ILoggerHelper _logger;
        public IGroupRoleService _srvGroupRole;
        private readonly IRepository<User> _repUser;
        private readonly IRepositoryConfig<UserConfig> _repConfig;
        public string CreateUser = "admin";
        public AccountController(
            IRepositoryConfig<UserConfig> repConfig,
            IRepository<User> repUser,
            IAccountService srv,
            IViewRendererHelper viewRender,
            ILoggerHelper logger,
            IGroupRoleService srvGroupRole)
        {
            _repConfig = repConfig;
            _repUser = repUser;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
            _srvGroupRole = srvGroupRole;
        }
        public IActionResult NoPermission()
        {
            return View();
        }
    }
}
