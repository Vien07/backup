
using Admin.LayoutPage.Models;
using Admin.LayoutPage.Services;
using Microsoft.AspNetCore.Mvc;

using NLog;

namespace Admin.LayoutPage.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiHomePageController : Controller
    {
        private readonly ILogger _logger;
        private readonly IHomePageService _srv;
        public ApiHomePageController(IHomePageService srv)
        {
            _srv = srv;
        }
        [HttpPost("GetHomePage")]
        public dynamic GetHomePageHTML()
        {
            var rs = _srv.GenerateHomePageHtml(0);
            return rs;
        }      
    }

}
