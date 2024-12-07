
using Admin.PostsManagement.Api.Models;
using Admin.PostsManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Admin.PostsManagement.Services;

namespace Admin.PostsManagement.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiPostsManagementController : Controller
    {

        private readonly ILogger _logger;
        private readonly IApiPostsManagementService _srv;

        public ApiPostsManagementController(IApiPostsManagementService srv)
        {
            _srv =    srv;
        }

        [HttpPost("GetPostBySlug")]
        public dynamic GetPostBySlug(Api.Models.Request.GetPostBySlugRequest input)
        {
            var rs = _srv.GetPostBySlug(input);
            return rs;
        }
        [HttpPost("GetListPostByCateSlug")]
        public dynamic GetListPostByCateSlug(Api.Models.Request.GetListPostBySlug input)
        {
            var rs = _srv.GetListPostByCateSlug(input);
            return rs;
        }  
        [HttpPost("GetListNewPostByCateSlug")]
        public dynamic GetListNewPostByCateSlug(Api.Models.Request.GetListNewPostByCateSlug input)
        {
            var rs = _srv.GetListNewPostByCateSlug(input);
            return rs;
        }   
        [HttpPost("GetListRelatePostByPostSlug")]
        public dynamic GetListRelatePostByPostSlug(Api.Models.Request.GetListRelatePostByPostSlug input)
        {
            var rs = _srv.GetListRelatePostByPostSlug(input);
            return rs;
        }
    }

}
