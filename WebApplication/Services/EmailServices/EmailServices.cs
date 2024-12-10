using CMS.Areas.Contact.Models;
using CMS.Repository;
using CMS.Services.CommonServices;
using CMS.Services.TranslateServices;
using CmsModels.EmailModels;
using DTO;
using DTO.Cart;
using DTO.Recruitment;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Services.EmailServices
{
    public class EmailServices : IEmailServices
    {
        private string EmailAdmin = "";
        private string RootDomain = "";
        private string Logo = "";
        private string EmailSMTPServer = "";
        private string EmailPort = "";
        private string EmailSMTPUser = "";
        private string EmailSMTPPassword = "";
        private string EmailFromName = "";
        private string EmailFromEmail = "";
        private string EmailEncryption = "";

        private readonly ICommonServices _common;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITranslateServices _translate;
        private readonly IContact_Repository _repContact;
        private readonly DBContext _dbContext;
        private readonly Queue<Func<Task>> _emailQueue = new Queue<Func<Task>>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private string KeyEmailAdmin = ConstantStrings.KeyEmailAdmin;
        private string KeyRootDomain = ConstantStrings.KeyRootDomain;
        private string KeyLogo = ConstantStrings.KeyLogo;
        private string KeyEmailSMTPServer = ConstantStrings.KeyEmailSMTPServer;
        private string KeyEmailPort = ConstantStrings.KeyEmailPort;
        private string KeyEmailSMTPUser = ConstantStrings.KeyEmailSMTPUser;
        private string KeyEmailSMTPPassword = ConstantStrings.KeyEmailSMTPPassword;
        private string KeyEmailFromName = ConstantStrings.KeyEmailFromName;
        private string KeyEmailFromEmail = ConstantStrings.KeyEmailFromEmail;
        private string UrlEmailTemplate = ConstantStrings.UrlEmailTemplate;
        private string TemplateEmail = ConstantStrings.TemplateEmail;
        private string UrlConfigurationImages = ConstantStrings.UrlConfigurationImages;
        private string UrlProductImages = ConstantStrings.UrlProductImages;
        private string TemplateEmailCus = ConstantStrings.TemplateEmailCus;
        private string TemplateEmailAdmin = ConstantStrings.TemplateEmailAdmin;
        private string EmailHeader = "";
        private string EmailFooter = "";
        public EmailServices(ICommonServices common, IHttpContextAccessor httpContextAccessor, ITranslateServices translate, IContact_Repository repContact, DBContext dbContext)
        {
            _common = common;
            _httpContextAccessor = httpContextAccessor;
            _translate = translate;
            _repContact = repContact;
            _dbContext = dbContext;

            EmailAdmin = _common.GetConfigValue(KeyEmailAdmin);
            RootDomain = _common.GetConfigValue(KeyRootDomain);
            Logo = _common.GetConfigValue(KeyLogo);
            EmailSMTPServer = _common.GetConfigValue(KeyEmailSMTPServer);
            EmailPort = _common.GetConfigValue(KeyEmailPort);
            EmailSMTPUser = _common.GetConfigValue(KeyEmailSMTPUser);
            EmailSMTPPassword = _common.GetConfigValue(KeyEmailSMTPPassword);
            EmailFromName = _common.GetConfigValue(KeyEmailFromName);
            EmailFromEmail = _common.GetConfigValue(KeyEmailFromEmail);
            EmailHeader = _common.GetConfigValue(ConstantStrings.KeyEmailGlobalEmailHeader);
            EmailFooter = _common.GetConfigValue(ConstantStrings.KeyEmailGlobalEmailFooter);
            EmailEncryption = _common.GetConfigValue(ConstantStrings.KeyEmailEncryption);

        }
        public string FillDataEmail(string rs, List<EmailVariableDto> EmailVariables)
        {
            try
            {
                string valueString = _common.GetWordsInStringWithSymbol(rs, "{{", "}}");
                string[] listValue = valueString.Split(';');
                foreach (var item in listValue)
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(item))
                        {
                            var temp = EmailVariables.Where(p => p.Code == item).FirstOrDefault();
                            if (temp != null)
                            {
                                string repaceValue = Convert.ToString(temp.Value);
                                rs = rs.Replace(Convert.ToString(item), repaceValue);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return rs;
            }
            catch (Exception ex)
            {
                return rs;
            }
        }
        public string FillDataEmailHeader(string rs, List<EmailVariableDto> EmailVariables)
        {
            try
            {
                var logo = EmailVariables.Where(x => x.Code == "{{Logo}}").Select(x => x.Value).FirstOrDefault();
                rs = rs.Replace("{logo}", logo);

                var siteName = EmailVariables.Where(x => x.Code == "{{CompanyName}}").Select(x => x.Value).FirstOrDefault();
                rs = rs.Replace("{sitename}", siteName);

                var address = EmailVariables.Where(x => x.Code == "{{Address}}").Select(x => x.Value).FirstOrDefault();
                rs = rs.Replace("{address}", address);

                var phone = EmailVariables.Where(x => x.Code == "{{Hotline}}").Select(x => x.Value).FirstOrDefault();
                rs = rs.Replace("{phone}", phone);

                rs = rs.Replace("{email}", EmailAdmin);

                return rs;
            }
            catch (Exception ex)
            {
                return rs;
            }
        }

        //admin
        public void SendRecoveryPassword(dynamic data)
        {
            try
            {
                string _host = _httpContextAccessor.HttpContext.Request.Host.Host.ToString();

                var lang = _httpContextAccessor.HttpContext.Session.GetString("WebsiteLang");
                var contactInfo = _repContact.GetContactInfo(lang);
                string companyName = Convert.ToString(contactInfo["contact-companyName"]);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(EmailFromName, EmailFromEmail));
                message.To.Add(new MailboxAddress(data.Email, data.Email));
                message.Subject = companyName + " | " + "Yêu Cầu Thay Đổi Mật Khẩu";
                var absolutepath = Directory.GetCurrentDirectory();

                var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                if (File.Exists(template))
                {
                    StreamReader sr = new StreamReader(template);
                    string content = sr.ReadToEnd();
                    content = content.Replace("{CompanyName}", companyName);
                    content = content.Replace("{Domain}", RootDomain);
                    content = content.Replace("{Url}", (!_httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://") + _host);
                    content = content.Replace("{Logo}", UrlConfigurationImages + Logo);

                    content = content.Replace("{FullName}", data.FullName);
                    content = content.Replace("{Content}", "<a href='//" + _httpContextAccessor.HttpContext.Request.Host.Value.ToString() + "/b-admin/Admin/ValidateRecoveryPassword?key=" + data.RecoveryString + "'>Xác nhận thay đổi mật khẩu</a>");
                    content = content.Replace("{ContactDate}", DateTime.Now.ToString());


                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = content;

                    message.Body = bodyBuilder.ToMessageBody();

                    QueueEmailSending(async () =>
                    {
                        using (var client = new SmtpClient())
                        {
                            client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                            await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                            client.AuthenticationMechanisms.Remove("XOAUTH2");
                            await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                            await client.SendAsync(message);
                            await client.DisconnectAsync(true);
                        }
                    });
                }

            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }
        public void SendNewPassword(dynamic data)
        {
            try
            {
                string _host = _httpContextAccessor.HttpContext.Request.Host.Host.ToString();

                var lang = _httpContextAccessor.HttpContext.Session.GetString("WebsiteLang");
                var contactInfo = _repContact.GetContactInfo(lang);
                string companyName = Convert.ToString(contactInfo["contact-companyName"]);

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(EmailFromName, EmailFromEmail));
                message.To.Add(new MailboxAddress(data.Email, data.Email));
                message.Subject = companyName + " | " + "Yêu Cầu Thay Đổi Mật Khẩu";
                var absolutepath = Directory.GetCurrentDirectory();

                var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                if (File.Exists(template))
                {
                    StreamReader sr = new StreamReader(template);
                    string content = sr.ReadToEnd();
                    content = content.Replace("{CompanyName}", companyName);
                    content = content.Replace("{Domain}", RootDomain);
                    content = content.Replace("{Url}", (!_httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://") + _host);
                    content = content.Replace("{Logo}", UrlConfigurationImages + Logo);

                    content = content.Replace("{FullName}", data.FullName);
                    content = content.Replace("{Content}", "Mật khẩu: " + data.Password + Environment.NewLine + "<a href = '//" + _httpContextAccessor.HttpContext.Request.Host.Value.ToString() + "/b-admin/'>Đăng nhập</a>");
                    content = content.Replace("{ContactDate}", DateTime.Now.ToString());

                    var bodyBuilder = new BodyBuilder();

                    bodyBuilder.HtmlBody = content;

                    message.Body = bodyBuilder.ToMessageBody();


                    QueueEmailSending(async () =>
                    {
                        using (var client = new SmtpClient())
                        {
                            client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                            await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                            client.AuthenticationMechanisms.Remove("XOAUTH2");
                            await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                            await client.SendAsync(message);
                            await client.DisconnectAsync(true);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }

        //contact
        public void SendToAdmin(ContactList data)
        {
            try
            {
                string _host = RootDomain;
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailContactToAdmin").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();

                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);


                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Address}}", Value = Convert.ToString(contactInfo["contact-address"]) });

                    emailVariables.Add(new EmailVariableDto() { Code = "{{FullName}}", Value = data.FullName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{PhoneNumber}}", Value = data.Phone });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Email}}", Value = data.Email });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Title}}", Value = data.Subject });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Content}}", Value = data.Content });

                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);

                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(EmailAdmin, EmailAdmin));
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();

                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {
                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        message.Body = bodyBuilder.ToMessageBody();

                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }
        public void SendToCus(ContactList data)
        {
            try
            {
                string _host = RootDomain;
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailContactToCustomer").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();
                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);


                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Address}}", Value = Convert.ToString(contactInfo["contact-address"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{ClientName}}", Value = data.FullName });

                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);
                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(data.FullName, data.Email));
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();

                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {
                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        message.Body = bodyBuilder.ToMessageBody();

                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }

        //enquire
        public void EnquireToAdmin(EnquireList data)
        {
            try
            {
                string _host = RootDomain;
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailEnquireToAdmin").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();

                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);


                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Address}}", Value = Convert.ToString(contactInfo["contact-address"]) });

                    emailVariables.Add(new EmailVariableDto() { Code = "{{FullName}}", Value = data.FullNameEnquire });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{PhoneNumber}}", Value = data.PhoneEnquire });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Email}}", Value = data.EmailEnquire });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{ServiceName}}", Value = data.ServiceName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Content}}", Value = data.ContentEnquire });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{EnquireDate}}", Value = data.DateEnquire.ToString("dd/MM/yyyy") });

                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);

                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(EmailAdmin, EmailAdmin));
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();

                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {
                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        message.Body = bodyBuilder.ToMessageBody();

                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }
        public void EnquireToCus(EnquireList data)
        {
            try
            {
                string _host = RootDomain;
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailEnquireToCustomer").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();
                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);


                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Address}}", Value = Convert.ToString(contactInfo["contact-address"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{ClientName}}", Value = data.FullNameEnquire });

                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);
                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(data.FullNameEnquire, data.EmailEnquire));
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();

                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {
                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        message.Body = bodyBuilder.ToMessageBody();

                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }

        //customer
        public async Task<bool> SendMailActiveCustomer(string email)
        {
            try
            {
                string _host = RootDomain;
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailActiveAccount").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();

                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);

                    var url = (!_httpContextAccessor.HttpContext.Request.IsHttps ? "http://" : "https://") + _host;

                    var random = new Random();
                    var randomCode = random.Next(100000, 600000).ToString();
                    var link = _host + _translate.GetUrl("url.active-email") + "?email=" + HttpUtility.UrlEncode(email) + "&code=" + randomCode;
                    var customer = await _dbContext.Customers.Where(x => !x.Deleted && x.Email.Equals(email)).FirstOrDefaultAsync();
                    if (customer != null)
                    {
                        customer.ActivationCode = randomCode;
                        await _dbContext.SaveChangesAsync();
                    }

                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Address}}", Value = Convert.ToString(contactInfo["contact-address"]) });

                    emailVariables.Add(new EmailVariableDto() { Code = "{{LinkActiveAccount}}", Value = link });

                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);
                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);


                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(email, email));
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();

                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {
                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        message.Body = bodyBuilder.ToMessageBody();

                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return false;
            }
        }
        public bool SendMailForgotPasswordCustomer(string email, string newPassword)
        {
            try
            {
                string _host = RootDomain;
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailForgotPassword").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();

                if (emailTemplate != null)
                {

                    var contactInfo = _repContact.GetContactInfo(lang);

                    var url = (!_httpContextAccessor.HttpContext.Request.IsHttps ? "http://" : "https://") + _host;

                    var random = new Random();
                    var randomCode = random.Next(100000, 600000).ToString();



                    var customer = _dbContext.Customers.Where(x => !x.Deleted && x.Email.Equals(email) && x.Enabled).FirstOrDefault();
                    if (customer != null)
                    {
                        customer.ActivationCode = randomCode;
                        _dbContext.SaveChanges();
                    }

                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Address}}", Value = Convert.ToString(contactInfo["contact-address"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{NewPassword}}", Value = newPassword });

                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);
                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);


                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(email, email));
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();

                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {
                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        message.Body = bodyBuilder.ToMessageBody();

                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
                return false;
            }
        }

        //recruitment
        public void SendRecruitToAdmin(CVDto data)
        {
            try
            {
                string _host = RootDomain;
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailRecruitToAdmin").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();

                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);


                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Address}}", Value = Convert.ToString(contactInfo["contact-address"]) });

                    emailVariables.Add(new EmailVariableDto() { Code = "{{FullName}}", Value = data.FullName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{PhoneNumber}}", Value = data.PhoneNumber });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Email}}", Value = data.Email });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Job}}", Value = data.NameRecruit });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Title}}", Value = data.Title });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Content}}", Value = data.Content });



                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);

                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(EmailAdmin, EmailAdmin));
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();

                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {
                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        message.Body = bodyBuilder.ToMessageBody();

                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }
        public void SendRecruitToCus(CVDto data)
        {
            try
            {
                string _host = RootDomain;
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }
                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailRecruitToCustomer").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();

                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);


                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Address}}", Value = Convert.ToString(contactInfo["contact-address"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{ClientName}}", Value = data.FullName });

                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);
                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(data.FullName, data.Email));
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();

                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {
                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        message.Body = bodyBuilder.ToMessageBody();

                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }

        public async Task SendMailOrderAdmin(long pid)
        {
            try
            {
                var enumStateOrderList = new Dictionary<int, string>();
                var stateOrderList = Enum.GetNames(typeof(EnumOrder.OrderState));
                for (var i = 0; i < stateOrderList.Length; i++)
                {
                    enumStateOrderList.Add(i, stateOrderList[i]);
                }

                var enumPaymentMethodList = new Dictionary<int, string>();
                var paymentMethodList = Enum.GetNames(typeof(EnumOrder.PaymentMethod));
                for (var i = 0; i < paymentMethodList.Length; i++)
                {
                    enumPaymentMethodList.Add(i, paymentMethodList[i]);
                }


                string _host = RootDomain;
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }

                var orderDetail = await _dbContext.Orders.Where(x => x.Pid == pid).FirstOrDefaultAsync();
                var orderList = await _dbContext.OrderDetails.Where(x => x.OrderPid == orderDetail.Pid).ToListAsync();
                var customer = await _dbContext.Customers.Where(x => x.Pid == orderDetail.CustomerPid).FirstOrDefaultAsync();

                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailAdminOrder").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();

                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);


                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyAddress}}", Value = Convert.ToString(contactInfo["contact-address"]) });

                    emailVariables.Add(new EmailVariableDto() { Code = "{{FirstName}}", Value = customer.FirstName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{LastName}}", Value = customer.LastName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FullName}}", Value = customer.FirstName + " " + customer.LastName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{PhoneNumber}}", Value = customer.PhoneNumber });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Email}}", Value = customer.Email });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CustomerAddress}}", Value = String.Format("{0}, {1}, {2}, {3}", customer.Address, _common.GetWard(customer.District, customer.Ward), _common.GetDistrict(customer.Province, customer.District), _common.GetProvince(customer.Province)) });

                    emailVariables.Add(new EmailVariableDto() { Code = "{{State}}", Value = enumStateOrderList.GetValueOrDefault(orderDetail.State) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{IsPayment}}", Value = !orderDetail.IsPayment ? "Chưa thanh toán" : "Đã thanh toán" });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{PaymentMethod}}", Value = enumPaymentMethodList.GetValueOrDefault(orderDetail.PaymentMethod) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{ShipFee}}", Value = _common.ConvertFormatMoney(orderDetail.ShipFee) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Deposit}}", Value = _common.ConvertFormatMoney(orderDetail.Deposit) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Total}}", Value = _common.ConvertFormatMoney(orderDetail.Total) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Note}}", Value = orderDetail.Note });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{InvoiceCode}}", Value = "#" + orderDetail.Pid.ToString() });




                    var html = "<table class=\"table-invoid fs-14 mb-2\">";

                    html += "<thead class=\"thead\"> <tr class=\"tr\">";
                    html += "<th class=\"th text-left\">Sản phẩm</th>";
                    html += "<th class=\"th text-right\">Đơn giá</th>";
                    html += "<th class=\"th text-right\">Số lượng</th>";
                    html += "<th class=\"th text-right\">Thành tiền</th>";
                    html += "</tr></thead>";
                    html += "<tbody class=\"tbody\">";
                    var listProduct = (from a in _dbContext.ProductDetails
                                       join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                       where (!a.Deleted)
                                       select new
                                       {
                                           Title = b.Title,
                                           Pid = a.Pid,
                                           PicThumb = a.PicThumb,
                                       }).ToList();

                    var product = listProduct.Where(x => x.Pid == orderDetail.ProductDetailPid).FirstOrDefault();
                    var productDetail = _dbContext.ProductCate_ProductDetails.Where(x => x.ProductDetailPid == orderDetail.ProductDetailPid).FirstOrDefault();
                    var months = _dbContext.ProductCates.Where(a => a.Pid == orderDetail.ProductCatePid).Select(x => x.Months).FirstOrDefault();
                    if (product != null)
                    {
                        html += string.Format("<tr class=\"tr\"><td class=\"td\">{0}</td><td class=\"td text-right\">{1}</td><td class=\"td text-right\">{2}</td><td class=\"td text-right\">{3}</td></tr>",
                            product.Title,
                            _common.ConvertFormatMoney(productDetail.Price) + "đ",
                            months + " tháng",
                            _common.ConvertFormatMoney(orderDetail.Total) + "đ");
                    }
                    html += "<tr class=\"bg-light fw-bold text-red\"><td class=\"td p-sm text-right\" colspan=\"3\">Tổng giá trị đơn hàng</td><td class=\"td p-sm text-right\">" + _common.ConvertFormatMoney(orderDetail.Total) + "đ</td>";
                    html += "</tbody>";
                    html += "</table>";

                    emailVariables.Add(new EmailVariableDto() { Code = "{{TableProductList}}", Value = html });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{TemporaryPrice}}", Value = _common.ConvertFormatMoney(orderList.Sum(x => x.Quantity * x.Price)) });

                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);
                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);

                    var message = new MimeMessage();

                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(customer.FirstName + " " + customer.LastName, customer.Email)); ;
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();
                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {
                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        message.Body = bodyBuilder.ToMessageBody();

                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }
        public async Task SendMailVAT(long pid, string path)
        {
            try
            {

                string _host = RootDomain;
                var lang = ConstantStrings.DefaultLang;


                var orderDetail = await _dbContext.Orders.Where(x => x.Pid == pid).FirstOrDefaultAsync();
                var orderList = await _dbContext.OrderDetails.Where(x => x.OrderPid == orderDetail.Pid).ToListAsync();
                var customer = await _dbContext.Customers.Where(x => x.Pid == orderDetail.CustomerPid).FirstOrDefaultAsync();

                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailAdminVAT").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();

                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);


                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Address}}", Value = Convert.ToString(contactInfo["contact-address"]) });

                    emailVariables.Add(new EmailVariableDto() { Code = "{{InvoiceCode}}", Value = "#" + orderDetail.Pid.ToString() });


                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);
                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);

                    var message = new MimeMessage();

                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(customer.FirstName + " " + customer.LastName, customer.Email)); ;
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();
                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {


                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        bodyBuilder.Attachments.Add(Path.GetFileName(path), File.ReadAllBytes(path), MimeKit.ContentType.Parse(MediaTypeNames.Application.Pdf));

                        message.Body = bodyBuilder.ToMessageBody();
                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }
        public async Task SendMailOrderCustomer(long pid)
        {
            try
            {

                string _host = RootDomain;
                var lang = ConstantStrings.DefaultLang;


                var orderDetail = await _dbContext.Orders.Where(x => x.Pid == pid).FirstOrDefaultAsync();
                var orderList = await _dbContext.OrderDetails.Where(x => x.OrderPid == orderDetail.Pid).ToListAsync();
                var customer = await _dbContext.Customers.Where(x => x.Pid == orderDetail.CustomerPid).FirstOrDefaultAsync();

                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailCustomerOrder").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();

                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);


                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Address}}", Value = Convert.ToString(contactInfo["contact-address"]) });

                    emailVariables.Add(new EmailVariableDto() { Code = "{{InvoiceCode}}", Value = "#" + orderDetail.Pid.ToString() });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FullName}}", Value = customer.FirstName + " " + customer.LastName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{PhoneNumber}}", Value = customer.PhoneNumber });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Email}}", Value = customer.Email });

                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);
                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);

                    var message = new MimeMessage();

                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(customer.FirstName + " " + customer.LastName, customer.Email)); ;
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();
                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {


                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;

                        message.Body = bodyBuilder.ToMessageBody();
                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }
        public async Task SendMailOrderToAdmin(long pid)
        {
            try
            {
                var enumStateOrderList = new Dictionary<int, string>();
                var stateOrderList = Enum.GetNames(typeof(EnumOrder.OrderState));
                for (var i = 0; i < stateOrderList.Length; i++)
                {
                    enumStateOrderList.Add(i, stateOrderList[i]);
                }

                var enumPaymentMethodList = new Dictionary<int, string>();
                var paymentMethodList = Enum.GetNames(typeof(EnumOrder.PaymentMethod));
                for (var i = 0; i < paymentMethodList.Length; i++)
                {
                    enumPaymentMethodList.Add(i, paymentMethodList[i]);
                }

                string _host = RootDomain;
                var lang = _httpContextAccessor.HttpContext.Session.GetString(ConstantStrings.WebsiteLang);
                if (string.IsNullOrEmpty(lang))
                {
                    lang = ConstantStrings.DefaultLang;
                }

                var orderDetail = await _dbContext.Orders.Where(x => x.Pid == pid).FirstOrDefaultAsync();
                var customer = await _dbContext.Customers.Where(x => x.Pid == orderDetail.CustomerPid).FirstOrDefaultAsync();

                var emailTemplate = (from a in _dbContext.EmailTemplates.Where(p => p.Code == "EmailOrderToAdmin").ToList()
                                     join b in _dbContext.MultiLang_EmailTemplates.Where(p => p.LangKey == lang).ToList()
                                     on a.Pid equals b.EmailTemplatePid
                                     select new
                                     {
                                         a.Title,
                                         b.Content,
                                         b.Subject,
                                         b.FromName
                                     }
                                 ).FirstOrDefault();

                if (emailTemplate != null)
                {
                    var contactInfo = _repContact.GetContactInfo(lang);



                    List<EmailVariableDto> emailVariables = new List<EmailVariableDto>();
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Logo}}", Value = RootDomain + UrlConfigurationImages + Logo });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Hotline}}", Value = Convert.ToString(contactInfo["contact-hotline"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyName}}", Value = Convert.ToString(contactInfo["contact-companyName"]) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{DatetimeNow}}", Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FromName}}", Value = EmailFromName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CompanyAddress}}", Value = Convert.ToString(contactInfo["contact-address"]) });

                    emailVariables.Add(new EmailVariableDto() { Code = "{{FirstName}}", Value = customer.FirstName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{LastName}}", Value = customer.LastName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{FullName}}", Value = customer.FirstName + " " + customer.LastName });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{PhoneNumber}}", Value = customer.PhoneNumber });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Email}}", Value = customer.Email });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{CustomerAddress}}", Value = String.Format("{0}, {1}, {2}, {3}", customer.Address, _common.GetWard(customer.District, customer.Ward), _common.GetDistrict(customer.Province, customer.District), _common.GetProvince(customer.Province)) });

                    emailVariables.Add(new EmailVariableDto() { Code = "{{State}}", Value = enumStateOrderList.GetValueOrDefault(orderDetail.State) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{IsPayment}}", Value = !orderDetail.IsPayment ? "Chưa thanh toán" : "Đã thanh toán" });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{PaymentMethod}}", Value = enumPaymentMethodList.GetValueOrDefault(orderDetail.PaymentMethod) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{ShipFee}}", Value = _common.ConvertFormatMoney(orderDetail.ShipFee) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Deposit}}", Value = _common.ConvertFormatMoney(orderDetail.Deposit) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Total}}", Value = _common.ConvertFormatMoney(orderDetail.Total) });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{Note}}", Value = orderDetail.Note });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{InvoiceCode}}", Value = "#" + orderDetail.Pid.ToString() });


                    var html = "<table class=\"table-invoid fs-14 mb-2\">";

                    html += "<thead class=\"thead\"> <tr class=\"tr\">";
                    html += "<th class=\"th text-left\">Sản phẩm</th>";
                    html += "<th class=\"th text-right\">Đơn giá</th>";
                    html += "<th class=\"th text-right\">Số lượng</th>";
                    html += "<th class=\"th text-right\">Thành tiền</th>";
                    html += "</tr></thead>";
                    html += "<tbody class=\"tbody\">";
                    var listProduct = (from a in _dbContext.ProductDetails
                                       join b in _dbContext.MultiLang_ProductDetails on a.Pid equals b.ProductDetailPid
                                       where (!a.Deleted)
                                       select new
                                       {
                                           Title = b.Title,
                                           Pid = a.Pid,
                                           PicThumb = a.PicThumb,
                                       }).ToList();

                    var product = listProduct.Where(x => x.Pid == orderDetail.ProductDetailPid).FirstOrDefault();
                    var productDetail = _dbContext.ProductCate_ProductDetails.Where(x => x.ProductDetailPid == orderDetail.ProductDetailPid).FirstOrDefault();
                    var months = _dbContext.ProductCates.Where(a => a.Pid == orderDetail.ProductCatePid).Select(x => x.Months).FirstOrDefault();
                    if (product != null)
                    {
                        html += string.Format("<tr class=\"tr\"><td class=\"td\">{0}</td><td class=\"td text-right\">{1}</td><td class=\"td text-right\">{2}</td><td class=\"td text-right\">{3}</td></tr>",
                            product.Title,
                            _common.ConvertFormatMoney(productDetail.Price)+"đ",
                            months + " tháng",
                            _common.ConvertFormatMoney(orderDetail.Total)+"đ");
                    }
                    html += "<tr class=\"bg-light fw-bold text-red\"><td class=\"td p-sm text-right\" colspan=\"3\">Tổng giá trị đơn hàng</td><td class=\"td p-sm text-right\">"+ _common.ConvertFormatMoney(orderDetail.Total) + "đ</td>";
                    html += "</tbody>";
                    html += "</table>";

                    emailVariables.Add(new EmailVariableDto() { Code = "{{TableProductList}}", Value = html });
                    emailVariables.Add(new EmailVariableDto() { Code = "{{TemporaryPrice}}", Value = _common.ConvertFormatMoney(orderDetail.Total) });

                    string tempValueSubject = _common.GetWordsInStringWithSymbol(emailTemplate.Subject, "{{", "}}");
                    string[] listValueSubject = tempValueSubject.Split(';');
                    string subject = FillDataEmail(emailTemplate.Subject, emailVariables);
                    string body = FillDataEmail(emailTemplate.Content, emailVariables);
                    string fromName = FillDataEmail(emailTemplate.FromName, emailVariables);
                    EmailHeader = FillDataEmailHeader(EmailHeader, emailVariables);
                    EmailFooter = FillDataEmailHeader(EmailFooter, emailVariables);
                    EmailFooter = EmailFooter.Replace("{address}", Convert.ToString(contactInfo["contact-address"]));

                    var message = new MimeMessage();

                    message.From.Add(new MailboxAddress(fromName, EmailFromEmail));
                    message.To.Add(new MailboxAddress(EmailAdmin, EmailAdmin)); ;
                    message.Subject = subject;
                    var absolutepath = Directory.GetCurrentDirectory();
                    var template = Path.Combine(absolutepath + "\\wwwroot\\" + UrlEmailTemplate, TemplateEmail);

                    if (File.Exists(template))
                    {
                        StreamReader sr = new StreamReader(template);
                        string content = sr.ReadToEnd();
                        content = content.Replace("{{header}}", EmailHeader);
                        content = content.Replace("{{body}}", body);
                        content = content.Replace("{{footer}}", EmailFooter);
                        var bodyBuilder = new BodyBuilder();
                        bodyBuilder.HtmlBody = content;
                        message.Body = bodyBuilder.ToMessageBody();

                        QueueEmailSending(async () =>
                        {
                            using (var client = new SmtpClient())
                            {
                                client.ServerCertificateValidationCallback = (l, j, c, m) => true;
                                await client.ConnectAsync(EmailSMTPServer, Convert.ToInt32(EmailPort), EmailEncryption == "SSL" ? SecureSocketOptions.SslOnConnect : EmailEncryption == "TLS" ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                                client.AuthenticationMechanisms.Remove("XOAUTH2");
                                await client.AuthenticateAsync(EmailSMTPUser, EmailSMTPPassword);
                                await client.SendAsync(message);
                                await client.DisconnectAsync(true);
                            }
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                _common.SaveLogError(ex);
            }
        }

        public void QueueEmailSending(Func<Task> emailSendingTask)
        {
            _emailQueue.Enqueue(emailSendingTask);
            StartSendingEmails();
        }

        private async void StartSendingEmails()
        {
            await _semaphore.WaitAsync();
            try
            {
                while (_emailQueue.Count > 0)
                {
                    var emailTask = _emailQueue.Dequeue();
                    await emailTask();
                }
            }
            catch (Exception ex)
            {
                //_common.SaveLogError(ex);
            }
            finally
            {
                _semaphore.Release();
            }
        }

    }
}
