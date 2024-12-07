
using Admin.TemplatePage.Models;
using Admin.TemplatePage.Services;
using Microsoft.AspNetCore.Mvc;

using NLog;

namespace Admin.TemplatePage.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiTemplatePageController : Controller
    {
        private readonly ILogger _logger;
        private readonly IApiTemplatePageService _srv;
        public ApiTemplatePageController(IApiTemplatePageService srv)
        {
            _srv = srv;
        }
        [HttpGet("GetDefaultMeta")]
        public dynamic GetDefaultMeta(string controller, string action)
        {
            var rs = _srv.GetDefaultMeta(controller, action);
            return rs;
        }
    }

}
