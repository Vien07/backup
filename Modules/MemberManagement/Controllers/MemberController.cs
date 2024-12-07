using Admin.MemberManagement;
using Admin.MemberManagement.Models;
using Admin.MemberManagement.Services;
using ComponentUILibrary.Models;
using MemberManagementManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Diagnostics;
using X.PagedList;

namespace Admin.MemberManagement.Controllers
{

    public partial class MemberManagementController : Controller
    {
        public IViewRendererHelper _viewRender;

        public IMemberManagementService _srv;
        public ILoggerHelper _logger;
        private string CreateUser = "admin";
        private readonly IRepositoryConfig<Database.MemberManagementConfig> _repConfig;
        private readonly IRepository<Database.MemberManagement> _repMemberManagement;
        private readonly IRepository<Database.MemberManagement_Files> _repMemberManagement_Files;
        public MemberManagementController(
            IMemberManagementService srv,
            IViewRendererHelper viewRender,
            IRepositoryConfig<Database.MemberManagementConfig> repConfig,
            IRepository<Database.MemberManagement> repMemberManagement,
            IRepository<Database.MemberManagement_Files> repMemberManagement_Files,
            ILoggerHelper logger)
        {
            
            _repConfig = repConfig;
            _repMemberManagement = repMemberManagement;
            _repMemberManagement_Files = repMemberManagement_Files;
            _srv = srv;
            _logger = logger;
            _viewRender = viewRender;
        }
      
    }
}