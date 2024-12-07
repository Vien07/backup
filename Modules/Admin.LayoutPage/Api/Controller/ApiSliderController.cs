
using Admin.LayoutPage.Models;
using Admin.LayoutPage.Services;
using Microsoft.AspNetCore.Mvc;

using NLog;

namespace Admin.LayoutPage.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiSliderController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISliderService _srv;
        public ApiSliderController(ISliderService srv)
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
