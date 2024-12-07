using Admin.MemberManagement.Models;
using X.PagedList;
using Steam.Core.Base.Models;
using Admin.MemberManagement.Api.Models.Request;
using Admin.MemberManagement.Api.Models.Response;

namespace Admin.MemberManagement.Services
{
    public interface IApiMemberManagementService
    {
        public Response RegisterAccount(RegisterAccount input);
        public Response<dynamic> LoginAccount(LoginAccount input);
        public Response ForgottenPassword(RegisterAccount input);
        public Response ChangePassword(ChangePasswordAccount input);
        public void SendMailToMember(RegisterAccount input);
        public Response<MemberInfo> GetMemberInfo(string email);
        public Response<bool> SaveFeedback(Api.Models.Request.FeedbackRequest model);
    }
}