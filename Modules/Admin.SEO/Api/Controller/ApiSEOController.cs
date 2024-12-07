
using Admin.SEO.Models;
using Microsoft.AspNetCore.Mvc;
using Admin.SEO.Services;
using NLog;

namespace Admin.SEO.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiSEOController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISEOService _srv;
        public ApiSEOController(ISEOService srv)
        {
            _srv = srv;
        }
        [HttpPost("GetAllListSEOActive")]
        public dynamic GetAllListSEOActive()
        {
            var rs = _srv.GetAllListSEOActive();
            return rs;
        }
        [HttpPost("GetPostBySlug")]
        public dynamic GetPostBySlug(string postSlug)
        {
            var Search = new ParamSearch();
            var rs = _srv.GetPostBySlug(postSlug);
            return rs;
        }
    }

}
