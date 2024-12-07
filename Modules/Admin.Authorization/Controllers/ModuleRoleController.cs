using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Admin.Authorization.Services;
using Admin.Authorization.Database;
using Steam.Infrastructure.Repository;
namespace Admin.Authorization.Controllers
{
    
    public partial class ModuleRoleController: Controller
    {
        IViewRendererHelper _viewRender;
        public ILoggerHelper _logger;
        public IModuleRoleService _srv;
        public string CreateUser = "admin";
        private readonly IRepository<ModuleRole> _repModuleRole;
        private readonly IRepository<Group_ModuleRole> _repModuleRoleGroup;
        private readonly IRepositoryConfig<AuthorizationConfig> _repConfig;
        public ModuleRoleController(
            IRepository<ModuleRole> repModuleRole,
            IRepositoryConfig<AuthorizationConfig> repConfig,
            IRepository<Group_ModuleRole> repModuleRoleGroup,
            IViewRendererHelper viewRender, 
            ILoggerHelper logger,
            IModuleRoleService srv
            )
        {
            _repModuleRoleGroup = repModuleRoleGroup;
            _repModuleRole = repModuleRole;
            _repConfig = repConfig;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;
        }
    }
}
