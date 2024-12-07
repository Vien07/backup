
using Steam.Core.LocalizeManagement.Models;
using Microsoft.AspNetCore.Mvc;

using NLog;
using Steam.Core.LocalizeManagement.Services;

namespace Steam.Core.LocalizeManagement.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiLocalizeCultureController : Controller
    {
        private readonly ILogger _logger;
        private readonly ILocalizeCultureService _srv;
        public ApiLocalizeCultureController(ILocalizeCultureService srv)
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
