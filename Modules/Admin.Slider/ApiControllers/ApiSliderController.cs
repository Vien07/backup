
using Admin.Slider.Models;
using Microsoft.AspNetCore.Mvc;

using NLog;

namespace Admin.Slider.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiSliderController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISliderRepository _rep;
        public ApiSliderController(ISliderRepository rep)
        {
            _rep  =    rep;
        }
        [HttpPost("GetList")]
        public dynamic GetList()
        {
          var Search = new ParamSearch();
            var a = _rep.GetList(Search);
            return a;
        }
    }

}
