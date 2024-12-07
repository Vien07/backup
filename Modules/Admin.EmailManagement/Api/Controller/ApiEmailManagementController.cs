
using Admin.EmailManagement.Database;
using Admin.EmailManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Admin.EmailManagement;
using NLog;
using Admin.EmailManagement.Api.Models;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Models;

using Admin.EmailManagement.Services;
using Admin.EmailManagement.Servcies;
using Steam.Core.Common.SteamString;
namespace Admin.EmailManagement.Api.Controllers
{
    [ApiController, Route("/api/[controller]")]
    public class ApiEmailManagementController : Controller
    {
        private readonly ILogger _logger;
        private readonly IApiEmailManagementService _srv;
        private readonly IMailHelper _mailHelper;
        private readonly IEmailMailBoxService _mailbox;

        public ApiEmailManagementController(
            IApiEmailManagementService srv, 
            IMailHelper mailHelper,
            IEmailMailBoxService mailbox
            )
        {
            _srv = srv;
            _mailHelper = mailHelper;
            _mailbox = mailbox;
        }

        // Chỉ gửi email là với tittle và content trong gói tin, CẦN truyền EmailCode để lấy mail cấu hình
        [HttpPost("SendEmail")]
        public Response SendEmail(Models.Request.SendEmail input)
        {
            Response rs = new Response();
            string groupistEmailReceive = string.Join(',', input.EmailReceive);
            EmailMailBox emailMailBox = new EmailMailBox(input.Title, input.Content, groupistEmailReceive, isSendWithTemplate:false, input.EmailCode);
            try
            {
                var rsConfig = _srv.GetEmailConfig(input.EmailCode);
                emailMailBox.EmailAdminPid = rsConfig.Data.Pid;
                MailSettingModel mailSetting = new MailSettingModel(rsConfig.Data.Sender, rsConfig.Data.SmtpServer, rsConfig.Data.SmtpUser, rsConfig.Data.SmtpPassword, rsConfig.Data.Port, rsConfig.Data.EmailAddress);
                foreach (var item in input.EmailReceive)
                {
                    _mailHelper.SendMail(item, input.Title, input.Content, mailSetting);
                }
                rs.IsError = false;
                rs.StatusCode = 200;
                emailMailBox.CountSendSuccess += 1;
                _mailbox.Save(emailMailBox);
            }
            catch (Exception e)
            {
                emailMailBox.IsFirstSendSuccess = false;
                emailMailBox.ErrorDetail = e.Message;
                _mailbox.Save(emailMailBox);
                rs.IsError = true;
                rs.StatusCode = 500;
                rs.Message = e.Message;
            }
            
            return rs;
        }

        // Gửi với Template định nghĩa sẵn, không cần truyền title và content, chỉ cần truyền LIST KEY
        [HttpPost("SendEmailWithTemplate")]
        public Response SendEmailWithTemplate(Models.Request.SendEmailWithTemplate input)
        {
            Response rs = new Response();
            try
            {
                var rsConfig = _srv.GetEmailConfig(input.EmailCode);
                var rsTemplate = _srv.GetEmailTemplate(input.EmailCode);
                string groupistEmailReceive = string.Join(',', input.EmailReceive);

                EmailMailBox emailMailBox = new EmailMailBox(rsTemplate.Data.Title, rsTemplate.Data.Content, groupistEmailReceive, isSendWithTemplate: true, input.EmailCode);
                MailSettingModel mailSetting = new MailSettingModel(rsConfig.Data.Sender, rsConfig.Data.SmtpServer, rsConfig.Data.SmtpUser, rsConfig.Data.SmtpPassword, rsConfig.Data.Port, rsConfig.Data.EmailAddress);
                emailMailBox.EmailAdminPid = rsConfig.Data.Pid;
                try
                {
                    if (input.ListKey is not null)
                    {
                        if (rsTemplate.Data.Content.Contains("{{"))
                        {
                            foreach (var item in input.ListKey)
                            {
                                rsTemplate.Data.Content = rsTemplate.Data.Content.Replace(item.Key, item.Value).ToRemoveBreakSympol();
                            }
                        }
                    }
                    foreach (var item in input.EmailReceive)
                    {
                        _mailHelper.SendMail(item, rsTemplate.Data.Title, rsTemplate.Data.Content, mailSetting);
                    }
                    rs.StatusCode = 200;
                    rs.IsError = false;
                    emailMailBox.CountSendSuccess += 1;
                }
                catch (Exception e)
                {
                    emailMailBox.IsFirstSendSuccess = false;
                    emailMailBox.ErrorDetail = e.Message;
                    throw;
                }
                
                _mailbox.Save(emailMailBox);
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.StatusCode = 500;
                rs.Message = ex.Message;
            }
            
            return rs;
        }
        [HttpPost("GetEmailTemplate")]
        public Response<Models.Response.GetEmailTemplate> GetEmailTemplate(string emailCode)
        {
            return _srv.GetEmailTemplate(emailCode);
        }
    }

}
