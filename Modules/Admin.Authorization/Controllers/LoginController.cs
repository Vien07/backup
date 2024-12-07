using Admin.Authorization.Database;
using Admin.Authorization.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Net.Http;

namespace Admin.Authorization
{

    public class LoginController : Controller
    {
        public IHttpContextAccessor _context;
        private IUserService _srv;
        HttpContext _httpContext;
        public LoginController(
            IUserService srv, 
            IHttpContextAccessor context)
        {
            _srv = srv; 
            _context = context;
        }

        public IActionResult Index(string redirectUrl)
        {
            var user = Request.Cookies["auth-token"];
            if(user != null) 
            {
                return Redirect("/");
            }
            ViewBag.redirectUrl = redirectUrl;
            return View();
        }

        [HttpGet("Login/Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth-token");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult LoginUser(LoginUser user)
        {
            var res = _srv.Login(user.UserName, user.Password);
            if (!res.IsError)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(1), // Thiết lập thời gian sống của cookie
                    IsEssential = true, // Cookie này là bắt buộc, không thể tắt qua cài đặt trình duyệt
                };

                Response.Cookies.Append("auth-token", res.Data.Token, cookieOptions);
            }
            
            return new JsonResult(res);
        }
    }
}
