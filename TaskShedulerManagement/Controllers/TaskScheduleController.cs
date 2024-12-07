using Microsoft.AspNetCore.Mvc;

using System.Dynamic;

using System.Collections.Generic;
using ComponentUILibrary.Models;
using X.PagedList;

namespace TaskShedulerManagement.Controllers
{

    public class TaskScheduleController : Controller
    {
        #region Define
        public class PageModel
        {
            public PageTitleModel PageTitle = new PageTitleModel("Sample", "Danh sách", "fas fa-layer-group", "/Sample");




            public IPagedList<dynamic> List;
            public List<dynamic> Configs;
            public dynamic EditModel;
            public dynamic Search;
            public PaginationModel Pagination = new PaginationModel();



        }
        #endregion

        public TaskScheduleController()
        {


        }
        public IActionResult Index()
        {


            return View();
        }


    }


}
