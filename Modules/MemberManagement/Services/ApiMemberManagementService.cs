
using Admin.MemberManagement.Database;
using Admin.MemberManagement.Models;
using System.Reflection;

using X.PagedList;
using FluentValidation.Results;

using Newtonsoft.Json;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Common.SteamString;
using Admin.MemberManagement.Api.Models.Request;
using Admin.EmailManagement;
using Admin.EmailManagement.Api.Controllers;
using Admin.EmailManagement.Api.Models.Request;
using BCrypt.Net;
using Steam.Infrastructure.Repository;
using Admin.EmailManagement.Services;
using Admin.EmailManagement.Servcies;


namespace Admin.MemberManagement.Services
{
    public class ApiMemberManagementService : IApiMemberManagementService
    {
        private ILoggerHelper _logger;
        //private readonly IMemberManagementService _srv;
        private readonly IApiEmailManagementService _srvEmail;
        private readonly IEmailMailBoxService _mailboxEmail;
        private readonly IMailHelper _mailHelper;
        private readonly IRepository<Database.MemberManagement> _repMemberManagement;
        private readonly IRepository<Database.Feedback> _repFeedback;

        private string CreateUser = "admin";
        public ApiMemberManagementService(
            IRepository<Database.Feedback> repFeedback,
            IRepository<Database.MemberManagement> repMemberManagement,
           IApiEmailManagementService srvEmail,
            IMailHelper mailHelper,
            ILoggerHelper logger, 

            IEmailMailBoxService mailboxEmail)
        {
            _repFeedback = repFeedback;
            _repMemberManagement = repMemberManagement;
            //_srv = srv;
            _logger = logger;
            _mailHelper = mailHelper;
            _srvEmail = srvEmail;
            _mailboxEmail = mailboxEmail;
        }

        public Response<dynamic> LoginAccount(LoginAccount input)
        {
            Response<dynamic> rs = new Response<dynamic>();
            try
            {
                if(!string.IsNullOrEmpty(input.Email) && !string.IsNullOrEmpty(input.Password))
                {
                    var user = _repMemberManagement.Query().Where(s => s.Email == input.Email && s.Deleted == false).FirstOrDefault();
                    if(user != null && BCrypt.Net.BCrypt.Verify(input.Password, user.Password) == true)
                    {
                        RegisterAccount data = new RegisterAccount();
                        data.Email = user.Email;
                        data.FirstName = user.FirstName;
                        data.LastName = user.LastName;
                        rs.Data = data;
                        rs.Message = "Login Success";
                        rs.StatusCode = 200;
                    }
                    else
                    {
                        rs.StatusCode = 500;
                        rs.Message = "Email or Password is not valid!";
                    }
                }
                else
                {
                    rs.StatusCode = 500;
                    rs.Message = "Email or Password is not valid!";
                }
                
                rs.IsError = false;
            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }

        public Response RegisterAccount(RegisterAccount input)
        {
            Response rs = new Response();
            using (var transaction = _repMemberManagement.BeginTransaction())
            {
                try
                {
                    var user = _repMemberManagement.Query().Where(s => s.Email == input.Email && s.Deleted == false).FirstOrDefault();
                    if (user is null)
                    {
                        Database.MemberManagement data = new Database.MemberManagement();
                        data.Pid = 0;
                        data.CreateUser = CreateUser;
                        data.UpdateUser = CreateUser;
                        data.FirstName = input.FirstName;
                        data.LastName = input.LastName;
                        data.Phone = input.PhoneNumber;
                        data.Email = input.Email;
                        data.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);
                        //_srv.Save(data, null, null);

                        transaction.Commit();
                        SendMailToMember(input);

                        rs.Message = "Register account success";
                        rs.StatusCode = 200;
                    }
                    else
                    {
                        rs.StatusCode = 500;
                        rs.Message = "Email is exist, please try another email.";
                    }
                    rs.IsError = false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    rs.StatusCode = 500;
                    rs.Message = "Register failed. Please try again or send help to admin!";
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

                }
            }
            return rs;
        }

        public Response ForgottenPassword(RegisterAccount input)
        {
            Response rs = new Response();
            try
            {
                if (!string.IsNullOrEmpty(input.Email))
                {
                    var checkExistEmail = _repMemberManagement.Query().Where(s => s.Email == input.Email && s.Deleted == false).FirstOrDefault();
                    if (checkExistEmail != null)
                    {
                        checkExistEmail.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);
                        _repMemberManagement.SaveChanges();
                        SendMailToMember(input);
                        rs.Message = "Reset password success. Please check your mail to get new password.";
                        rs.StatusCode = 200;
                    }
                    else
                    {
                        rs.StatusCode = 500;
                        rs.Message = "Email does not exist";
                    }
                }
                else
                {
                    rs.StatusCode = 500;
                    rs.Message = "Email does not exist";
                }
                rs.IsError = false;
                return rs;
            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;
                rs.IsError = true;
                rs.Message = "Reset password failed. Please try again or send help to admin.";
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, "");

            }
            return rs;
        }

        public Response ChangePassword(ChangePasswordAccount input)
        {
            Response rs = new Response();
            try
            {
                var checkExistEmail = _repMemberManagement.Query().Where(s => s.Email == input.Email && s.Deleted == false).FirstOrDefault();
                if(checkExistEmail != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(input.Password, checkExistEmail.Password))
                    {
                        checkExistEmail.Password = BCrypt.Net.BCrypt.HashPassword(input.NewPassword);
                        _repMemberManagement.SaveChanges();
                        rs.Message = "Change password success";
                        rs.StatusCode = 200;
                    }
                    else
                    {
                        rs.StatusCode = 500;
                        rs.Message = "Password does not correct";
                    }
                }
                else
                {
                    rs.StatusCode = 500;
                    rs.Message = "Email does not exist";
                }
                rs.IsError = false;
            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;
                rs.IsError = true;
                rs.Message = "Change password failed. Please try again or send help to admin.";
            }
            return rs;
        }

        public Response<Admin.MemberManagement.Api.Models.Response.MemberInfo> GetMemberInfo(string email)
        {
            Response<Admin.MemberManagement.Api.Models.Response.MemberInfo> rs = new Response<Api.Models.Response.MemberInfo>();
            try
            {
                var checkExistEmail = _repMemberManagement.Query().Where(s => s.Email == email && s.Deleted == false).FirstOrDefault();
                if (checkExistEmail != null)
                {
                    rs.Data = new Api.Models.Response.MemberInfo();
                    rs.Data.FirstName = checkExistEmail.FirstName ?? null;
                    rs.Data.LastName = checkExistEmail.LastName ?? null;
                    rs.Data.Email = checkExistEmail.Email ?? null;
                    rs.Data.Phone = checkExistEmail.Phone ?? null;
                    rs.StatusCode = 200;
                    rs.Message = "Success";
                }
                else
                {
                    rs.StatusCode = 500;
                    rs.Message = "Email does not exist";
                }
                rs.IsError = false;
            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;
                rs.IsError = true;
                rs.Message = "Account does not exist";
            }
            return rs;
        }

        public void SendMailToMember(RegisterAccount input)
        {
            var apiEmailController = new ApiEmailManagementController(_srvEmail, _mailHelper, _mailboxEmail);

            SendEmailWithTemplate sendEmailWithTemplate = new SendEmailWithTemplate();
            sendEmailWithTemplate.ListKey = input.ListKey;
            sendEmailWithTemplate.EmailCode = input.EmailCode;
            sendEmailWithTemplate.EmailReceive = new string[] { input.Email };
            apiEmailController.SendEmailWithTemplate(sendEmailWithTemplate);
        }
        public Response<bool> SaveFeedback(Api.Models.Request.FeedbackRequest model)
        {
            Response<bool> rs = new Response<bool>();
            try
            {
                var newFeedback = new Feedback();
                newFeedback.FullName = model.FullName;
                newFeedback.Email = model.Email;
                newFeedback.SKU = model.SKU;
                newFeedback.Rating = model.Rating;
                newFeedback.Content = model.Content;
                newFeedback.CreateUser = "admin";
                newFeedback.UpdateUser = "admin";
                _repFeedback.Add(newFeedback);
                _repFeedback.SaveChanges();

                rs.Data = true; 
                rs.StatusCode = 200; 
            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;
                rs.IsError = true;
                rs.Message = ex.Message;
            }
            return rs;
        }
    }

}
