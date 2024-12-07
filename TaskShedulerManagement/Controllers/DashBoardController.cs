using Microsoft.AspNetCore.Mvc;

using System.Dynamic;

using System.Collections.Generic;

namespace TaskShedulerManagement.Controllers
{
 
    public class DashBoardController : Controller
    {
 

        public DashBoardController(   )
        {
     

        }
        public IActionResult Index()
        {            


            return View();
        }


    }


}
