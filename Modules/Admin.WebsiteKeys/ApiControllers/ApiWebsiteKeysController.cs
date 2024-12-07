
using Admin.WebsiteKeys.Models;
using Admin.WebsiteKeys.Service;
using Microsoft.AspNetCore.Mvc;

using NLog;

namespace Admin.WebsiteKeys.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiWebsiteKeysController : Controller
    {
        private readonly ILogger _logger;
        private readonly IWebsiteKeysService _srv;
        public ApiWebsiteKeysController(IWebsiteKeysService srv)
        {
            _srv = srv;
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
