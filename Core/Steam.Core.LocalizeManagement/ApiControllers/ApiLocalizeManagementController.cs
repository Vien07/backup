
using Steam.Core.LocalizeManagement.Models;
using Microsoft.AspNetCore.Mvc;

using NLog;
using Steam.Core.LocalizeManagement.Services;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
namespace Steam.Core.LocalizeManagement.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiLocalizeManagementController : Controller
    {
        public ILocalizeManagementService _srv;
        public ILoggerHelper _logger;
        public ApiLocalizeManagementController(
     IRepository<Database.LocalizeManagement> repLocalizeManagement,
    IRepositoryConfig<Database.LocalizeManagementConfig> repConfig,
    ILocalizeManagementService srv,
    IViewRendererHelper viewRender,
    ILoggerHelper logger)
        {
            _srv = srv;
            _logger = logger;
        }
        [HttpPost("GetList")]
        public dynamic GetList()
        {
            var Search = new ParamSearch();
            var a = _srv.GetList(Search);
            return a;
        }
    }

}
