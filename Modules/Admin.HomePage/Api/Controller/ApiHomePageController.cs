
using Admin.HomePage.Models;
using Microsoft.AspNetCore.Mvc;

using NLog;

namespace Admin.HomePage.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiHomePageController : Controller
    {
        private readonly ILogger _logger;
        private readonly IHomePageRepository _rep;
        public ApiHomePageController(IHomePageRepository rep)
        {
            _rep  =    rep;
        }
        [HttpPost("GetHomePage")]
        public dynamic GetHomePageHTML()
        {
            var rs = _rep.GetHomePageHTML();
            return rs;
        }      
    }

}
