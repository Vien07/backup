using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Orders.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderController(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PayOSSucess()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return View();
        }

        public IActionResult PayOSCancel()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return View();
        }
    }
}
