using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Reflection;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;

namespace Steam.Core.Utilities.STeamHelper
{
    public class MailHelper : IMailHelper
    {
        ILoggerHelper _logger;
        public MailHelper(ILoggerHelper logger)
        {
            _logger = logger;
        }
        public void SendMail(string mail, string subject, string htmlBody, MailSettingModel setting)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(setting.SmtpSender, setting.EmailAdmin));
                message.To.Add(new MailboxAddress(mail, mail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlBody;
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                    client.Connect(setting.SmtpServer, Convert.ToInt32(setting.SmtpPort), SecureSocketOptions.StartTls);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(setting.SmtpUser, setting.SmtpPassword);
                    client.Send(message);
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
            catch (Exception ex)
            {
                var obj = new { mail = mail, subject = subject, htmlBody = htmlBody };
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, obj.ToJson());
            }
        }
        public void SendMails(string mails, string subject, string htmlBody, MailSettingModel setting)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(setting.SmtpSender, setting.EmailAdmin));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlBody;
                message.Body = bodyBuilder.ToMessageBody();

                string[] emails = mails.Split(',');
                if (emails.Length > 0)
                {
                    foreach (var email in emails)
                    {
                        message.To.Add(new MailboxAddress(email, email));
                        using (var client = new SmtpClient())
                        {
                            client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                            client.Connect(setting.SmtpServer, Convert.ToInt32(setting.SmtpPort), SecureSocketOptions.StartTls);
                            client.AuthenticationMechanisms.Remove("XOAUTH2");
                            client.Authenticate(setting.SmtpUser, setting.SmtpPassword);
                            client.Send(message);
                            client.Disconnect(true);
                            client.Dispose();
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                var obj = new { mail = mails, subject = subject, htmlBody = htmlBody };
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, obj.ToJson());
            }
        }
    }
}
