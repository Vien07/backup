using Admin.LogManagement;
using Admin.LogManagement.Models;
using ComponentUILibrary.Models;
using LogManagementManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Steam.Core.Common.STeamHelper;
using System.Diagnostics;
using X.PagedList;

namespace Admin.LogManagement
{

    public partial class LogManagementController : Controller
    {
        public IViewRendererHelper _viewRender;

        public ILogManagementRepository _rep;
        public ILoggerHelper _logger;
        private string CreateUser = "admin";

        public LogManagementController(ILogManagementRepository rep, IViewRendererHelper viewRender, ILoggerHelper logger)
        {
            _rep = rep;
            _logger = logger;
            _viewRender = viewRender;
        }
      
    }
}