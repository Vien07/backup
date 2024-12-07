//using Admin.Sample.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Base;
using Steam.Core.Base.Constant;
using Steam.Core.Common;
using Newtonsoft.Json;
using RestSharp;
using Steam.Core.Base.Models;
using MasterAdmin.Repository;
using Admin.WebSetting.Constants;
using Admin.Authorization.Database;
using MasterAdmin.Models.DashboardViewModel;
using MasterAdmin.Models.Dashboard_ShortcutViewModel;
using Admin.DashBoard.Database;
using Steam.Core.Utilities.STeamHelper;
using Admin.DashBoard.Constants;

namespace MasterAdmin.DashBoard
{

    public class DashBoardController : Controller
    {
        string AppKey = "";
        IHttpContextAccessor _httpContext;
        public IViewRendererHelper _viewRender;
        private readonly IConfiguration configuration;
        private IDashBoardRepository _rep;
        Dictionary<string, string> _configWebsite;
        public class PageModel
        {
            public List<LogManagement> ListLogs = new List<LogManagement>();
            public List<Dashboard_Shortcut> ListShortcuts = new List<Dashboard_Shortcut>();
            public List<Product_Item> ListProductHots = new List<Product_Item>();
            public StatisticsInfo StatisticsInfo = new StatisticsInfo();
            public ReportTraffic ReportTrafficInDay = new ReportTraffic();
            public ReportTraffic ReportTrafficByDay = new ReportTraffic();
            public string CountTraffic = "";


        }
        public PageModel _pageModel = new PageModel();
        public UserModel CurrentUser;
        //public int dem = 0;

        public DashBoardController(IDashBoardRepository rep, IIdentityService indentitySrv, IHttpContextAccessor httpContext, IViewRendererHelper viewRender)
        {
            configuration = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
            _rep = rep;
            _configWebsite = rep.GetWebSiteConfig();
            AppKey = configuration["Website:AppKey"].ToString();
            _httpContext = httpContext;
            _viewRender = viewRender;
            //CurrentUser = indentitySrv.GetUser();
            //List<TheirCourse> test = new List<TheirCourse>();
            //for(int i=0;i<10;i++)
            //{
            //    test.Add(GenerateData(i,3,3));
            //}
            //var a = fromTheirCourses(test);
        }
        public IActionResult Index()
        {            //_multiLangService.ChangeConnectionString("Server=.;Database=cms_db;Integrated Security=True;MultipleActiveResultSets=True");
            var size = new DirectoryInfo(SystemInfo.AbsolutePathFileManager).GetDirectorySize();

            _pageModel.StatisticsInfo.SizeMedia = (size / 1048576).ToString();
            _pageModel.StatisticsInfo.OnlineUser = "0";// GetUserOnline(_configWebsite[WebSettingConstants.ConfigName.ApiGetUserOnline]).ToString();
            var inday = GetTrafficReport(
                DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"), "inday");
            _pageModel.ReportTrafficInDay = inday;
            var byday = GetTrafficReport(
             DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"), "byday");
            _pageModel.ReportTrafficByDay = byday;
            _pageModel.StatisticsInfo.CountTraffic = GetTraffic();
            _pageModel.StatisticsInfo.CountPost = _rep.GetNumberOfPost().ToString();
            _pageModel.ListLogs = _rep.GetLogs(10);
            _pageModel.ListProductHots = _rep.GetListHotProduct(5);
            //_pageModel.ListShortcuts = _rep.GetShortcuts(5);
            //ViewBag.ListShortcuts = _pageModel.ListShortcuts;
            return View(_pageModel);
        }
        public int GetUserOnline(string api)
        {

            try
            {
                Response<int> responseObj = new Response<int>();




                var client = new RestClient(api);
                var request = new RestRequest();
                request.Timeout = 1000;
                request.AddHeader(nameof(AppKey), AppKey);
                var response = client.ExecutePost(request);
                if (response.IsSuccessful)
                {
                    responseObj = JsonConvert.DeserializeObject<Response<int>>(response.Content);

                }
                else
                {
                    responseObj.Message = response.ErrorMessage.ToString();
                }
                return responseObj.Data;
            }
            catch (Exception ex)
            {

                return -1;
            }

        }
        public ReportTraffic GetTrafficReport(string fromDate, string toDate, string type)
        {
            ReportTraffic rs = new ReportTraffic();
            rs.Name = "[]";
            rs.Value = "[]";

            try
            {

                string api = _configWebsite[WebSettingConstants.ConfigName.ApiGetTrafficReport];
                Response<List<ReportTraffic>> responseObj = new Response<List<ReportTraffic>>();
                if (toDate == null)
                {
                    toDate = "";
                }


                var client = new RestClient(api);
                var request = new RestRequest($"?fromDate={fromDate}&toDate={toDate}&type={type}");
                request.Timeout = 1000;
                request.AddHeader(nameof(AppKey), AppKey);
                var response = client.ExecuteGet(request);
                if (response.IsSuccessful)
                {
                    responseObj = JsonConvert.DeserializeObject<Response<List<ReportTraffic>>>(response.Content);
                    List<ReportTraffic> data = responseObj.Data;
                    rs.ToValue(data);
                    rs.ToLabel(data);
                }
                else
                {
                    responseObj.Message = response.ErrorMessage.ToString();
                }
            }
            catch (Exception ex)
            {
                

            }
            return rs;

        }
        public string GetTraffic()
        {
            Response<string> responseObj = new Response<string>();

            try
            {
                string api = _configWebsite[WebSettingConstants.ConfigName.ApiGetTraffic];




                var client = new RestClient(api);
                var request = new RestRequest();
                request.Timeout = 1000;
                request.AddHeader(nameof(AppKey), AppKey);
                var response = client.ExecuteGet(request);
                if (response.IsSuccessful)
                {
                    responseObj = JsonConvert.DeserializeObject<Response<string>>(response.Content);

                }
                else
                {
                    responseObj.Message = response.ErrorMessage.ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return "165";// responseObj.Data;

        }
        public List<LogManagement> GetLogs()
        {
            List<LogManagement> logs = new List<LogManagement>();
            try
            {
                _pageModel.ListLogs = _rep.GetLogs(10);
            }
            catch (Exception ex)
            {

            }
            return logs;
        }

        public IActionResult SaveShortCut(ShortCutSaveModel data)
        {
            var res = _rep.SaveShortcuts(data);
            if (!res.IsError)
            {
                data.Pid = res.Data.Pid;
            }
            var listData = _viewRender.RenderPartialViewToString(DashBoardConstants.StaticPath.PartialView.ShortCut, _rep.GetShortcuts(5));
            return new JsonResult(new { response = res, listData = listData });
        }
        public List<Dashboard_Shortcut> GetShortCuts()
        {

            List<Dashboard_Shortcut> shortcuts = new List<Dashboard_Shortcut>();
            try
            {
                _pageModel.ListShortcuts = _rep.GetShortcuts(99);
            }
            catch (Exception ex)
            {

            }

            return shortcuts;
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {

            var res = _rep.Delete(id);

            //var listData = _viewRender.RenderPartialViewToString(PostsManagementConstants.StaticPath.PartialView.Index_Table, _rep.GetList(_pageModel.Search).Data);
            return new JsonResult(new { response = res, listData = "" });
        }
        public JsonResult EditModal(long id)
        {
            Dashboard_Shortcut res = new Dashboard_Shortcut();

            if (id != 0)
            {
                res = _rep.GetShortCutById(id);

            }


            string modalEdit = _viewRender.RenderPartialViewToString(DashBoardConstants.StaticPath.PartialView.ModalAdd_Shortcut, res);

            return new JsonResult(new { response = res, modal = modalEdit });
        }
        [HttpPost]

        public JsonResult UploadImage(IFormFile file)
        {
            FileHelper a = new FileHelper();
            var rs = a.UploadImage(file);

            return new JsonResult(new { location = "/abvc/d.png" });
        }
        //public class TheirCourse
        //{
        //    public long Id { get; set; }
        //    public string Name { get; set; }
        //    public List<TheirCourse> Children { get; set; }
        //}

        //public class Course
        //{
        //    public long Id { get; set; }
        //    public string Name { get; set; }
        //    public long ParentId { get; set; }
        //}
        //public void addToCourse(TheirCourse theirCourse, long ParentId, ref List<Course> rs)
        //{
        //    Course temp = new Course();
        //    if (theirCourse.Children != null && theirCourse.Children.Count > 0)
        //    {
        //        temp.Id = theirCourse.Id;
        //        temp.Name = theirCourse.Name;
        //        temp.ParentId = ParentId;
        //        rs.Add(temp);
        //        foreach (var item in theirCourse.Children)
        //        {
        //            addToCourse(item, theirCourse.Id, ref rs);

        //        }

        //    }
        //    else
        //    {
        //        temp.Id = theirCourse.Id;
        //        temp.Name = theirCourse.Name;
        //        temp.ParentId = ParentId;
        //        rs.Add(temp);

        //    }
        //}
        //public List<Course> fromTheirCourses(List<TheirCourse> theirCourses)
        //{
        //    try
        //    {
        //        List<Course> rs = new List<Course>();
        //        foreach (var item in theirCourses)
        //        {
        //            addToCourse(item, item.Id, ref rs);

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        return new List<Course>();
        //    }
        //    return new List<Course>();

        //}
        //public TheirCourse GenerateData(long id, int maxDepth, int maxChildren)
        //{
        //    if (maxDepth == 0)
        //    {
        //        return new TheirCourse { Id = id, Name = $"Course{id}" };
        //    }

        //    var course = new TheirCourse { Id = id, Name = $"Course{id}", Children = new List<TheirCourse>() };

        //    Random random = new Random();

        //    int numChildren = random.Next(1, maxChildren + 1);

        //    for (int i = 1; i <= numChildren; i++)
        //    {
        //        long childId = id * 10 + i; // Tạo id cho con dựa trên id của cha
        //        TheirCourse child = GenerateData(childId, maxDepth - 1, maxChildren);
        //        course.Children.Add(child);
        //        dem =dem+1;
        //    }

        //    return course;
        //}
    }


}
