using CMS.Areas.Admin;
using CMS.Areas.Shared.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMS.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _rep;
        private readonly DBContext _dBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminRepository _repAdmin;

        public DashboardController(DBContext dBContext, IHttpContextAccessor httpContextAccessor, IDashboardRepository rep, IAdminRepository repAdmin)
        {
            _rep = rep;
            _dBContext = dBContext;
            _httpContextAccessor = httpContextAccessor;
            _repAdmin = repAdmin;
        }
        //[CustomAuthorization("Dashboard", "VIEW")]
        public async Task<IActionResult> Index()
        {
            ViewBag.News = _rep.LoadDataNews();
            ViewBag.Product = _rep.LoadDataProduct();
            ViewBag.Visitor = await _rep.GetVisitor();
            ViewBag.CountNews = _rep.CountNews();
            ViewBag.CountProduct = _rep.CountProduct();
            ViewBag.DataChart = await _rep.GetBarChart(DateTime.Now.Month, DateTime.Now.Year);
            ViewBag.CountVisitorInMonth = await _rep.CountVisitorInMonth(DateTime.Now.Month, DateTime.Now.Year);
            ViewBag.Permission = _repAdmin.GetPermissionForUser();
            return View();

        }
        public async Task<JsonResult> GetBarChart(DateTime date)
        {
            var month = date.Month;
            var year = date.Year;
            return Json(new { js = await _rep.GetBarChart(month, year), total = await _rep.CountVisitorInMonth(month, year) });
        }
        public async Task<JsonResult> GetBarChartWithDate(DateTime startDate, DateTime endDate)
        {
            return Json(new { js = await _rep.GetBarChartWithDate(startDate, endDate), total = await _rep.CountVisitorWithDate(startDate, endDate) });
        }
    }
}