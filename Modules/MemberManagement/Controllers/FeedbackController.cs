//using Admin.PostsManagement.Models;
using Admin.MemberManagement;
using Admin.MemberManagement.Models;
using Admin.MemberManagement.Services;
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Base;
using Steam.Core.Utilities.STeamHelper;
using Steam.Infrastructure.Repository;
using System.Collections.Generic;
using System.Net.Http;
using X.PagedList;

namespace Admin.MemberManagement.Controllers
{

    public partial class FeedbackController : Controller
    {
        IHttpContextAccessor _httpContext;
        private readonly IRepositoryConfig<Database.FeedbackConfig> _repConfig;
        private readonly IRepository<Database.Feedback> _repFeedback;
        private readonly IRepository<Database.Feedback_Files> _repFeedback_Files;
        public IFeedbackService _srv;
        public ILoggerHelper _logger;
        public IViewRendererHelper _viewRender;
        public string CreateUser = "admin";

        public FeedbackPageModel _pageModel = new FeedbackPageModel();
        public FeedbackController(
            IRepositoryConfig<Database.FeedbackConfig> repConfig,
             IRepository<Database.Feedback> repFeedback,
              IRepository<Database.Feedback_Files> repFeedback_Files,
            IFeedbackService srv,
            IViewRendererHelper viewRender,
            ILoggerHelper logger,
            IHttpContextAccessor httpContext)
        {
            _repConfig = repConfig;
            _repFeedback = repFeedback;
            _repFeedback_Files = repFeedback_Files;
            _httpContext = httpContext;
            _viewRender = viewRender;
            _srv = srv;
            _logger = logger;

        }
    }

}
