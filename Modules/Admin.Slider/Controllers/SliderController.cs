using Admin.Slider;
using Admin.Slider.Models;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Common.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.Slider
{


    public partial class SliderController : Controller
    {
        public ISliderRepository _rep;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public PageModel _pageModel = new PageModel();
        public SliderController(ISliderRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _rep = rep;
            _logger = logger;
        }
    }

}
