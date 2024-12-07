using Admin.Authorization.Database;
using Admin.Authorization.Services;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;

namespace Admin.Authorization.Controllers
{
    
    public partial class GroupRoleController: Controller
    {
        IViewRendererHelper _viewRender;
        public ILoggerHelper _logger;
        public IGroupRoleService _srv;
        public IModuleRoleService _srvModuleRole;
        public string CreateUser = "admin";
        private readonly IRepository<GroupRole> _repGroupRoles;
        private readonly IRepository<Group_ModuleRole> _repModuleRoleGroup;
        private readonly IRepositoryConfig<AuthorizationConfig> _repConfig;
        public GroupRoleController(
            IRepository<GroupRole> repGroupRoles,
             IRepository<Group_ModuleRole> repModuleRoleGroup,
             IRepositoryConfig<AuthorizationConfig> repConfig,
            IViewRendererHelper viewRender,
            ILoggerHelper logger, 
            IGroupRoleService srv,
            IModuleRoleService srvModuleRole)
        {
            _repConfig = repConfig;
            _viewRender = viewRender;
            _srv = srv;
            _srvModuleRole = srvModuleRole;
            _logger = logger;
            _repGroupRoles = repGroupRoles;
            _repModuleRoleGroup = repModuleRoleGroup;
        }
    }
}
