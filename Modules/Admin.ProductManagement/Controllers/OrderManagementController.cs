using Admin.ProductManagement;
using Admin.ProductManagement.Models;
using Admin.ProductManagement.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Utilities.STeamHelper;
using System.Collections.Generic;
using X.PagedList;

namespace Admin.ProductManagement.Controllers
{
    public partial class OrderManagementController : Controller
    {
        public IOrderManagementService _service;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public PageModel _pageModel = new PageModel();
        public OrderManagementController(IOrderManagementService rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _viewRender = viewRender;
            _service = rep;
            _logger = logger;
        }
    }
}
