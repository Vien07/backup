using Admin.MemberManagement.Api.Models.Request;
using Admin.MemberManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Base.Models;

namespace Admin.MemberManagement.ApiControllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiMemberPageManagementController : Controller
    {
        private readonly IApiMemberManagementService _srv;
        public ApiMemberPageManagementController(IApiMemberManagementService srv)
        {

            _srv = srv;

        }

        [HttpPost("LoginAccount")]
        public dynamic LoginAccount(Api.Models.Request.LoginAccount input)
        {
            var rs = _srv.LoginAccount(input);
            return rs;
        }

        [HttpPost("RegisterAccount")]
        public Response RegisterAccount(Api.Models.Request.RegisterAccount input)
        {
            var rs = _srv.RegisterAccount(input);
            return rs;
        }

        [HttpPost("ForgottenPassword")]
        public Response ForgottenPassword(RegisterAccount input)
        {
            var rs = _srv.ForgottenPassword(input);
            return rs;
        }

        [HttpPost("ChangePassword")]
        public Response ChangePassword(ChangePasswordAccount input)
        {
            var rs = _srv.ChangePassword(input);
            return rs;
        }

        [HttpPost("GetMemberInfo")]
        public Response<Admin.MemberManagement.Api.Models.Response.MemberInfo> GetMemberInfo(string email)
        {
            var rs = _srv.GetMemberInfo(email);
            return rs;
        }

        [HttpPost("Feedback")]
        public Response<bool> Feedback([FromForm] FeedbackRequest model)
        {
            var rs = _srv.SaveFeedback(model);
            return rs;
        }
    }
}
