
using Admin.WebSetting.Services;
using Microsoft.AspNetCore.Mvc;

using NLog;
using Steam.Infrastructure.Repository;
using System.Text;
using System.Xml;

namespace Admin.WebSetting.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiWebSettingController : Controller
    {
        private readonly ILogger _logger;
        private readonly IRepository<Database.WebsiteConfiguration> _repWebsiteConfiguration;
        public IWebSettingService _srv;

        public ApiWebSettingController(
            IWebSettingService srv,
            IRepository<Database.WebsiteConfiguration> repWebsiteConfiguration
            )
        {
            _srv = srv;
            _repWebsiteConfiguration = repWebsiteConfiguration;
        }

    }

}
