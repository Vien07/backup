using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.WebSetting.Controllers
{
    public partial class WebSettingController 
    {
        public IActionResult Index()
        {
            _pageModel.EditModel = _srv.GetWebSetting().Data;
            return View(_pageModel);
        }
    }
}
