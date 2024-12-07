
using Microsoft.AspNetCore.Http;
using Admin.EmailManagement.Database;
using Admin.EmailManagement.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.EmailManagement.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;

using Steam.Infrastructure.Repository;
using Admin.EmailManagement.Servcies;

namespace Admin.EmailManagement.Services
{
    public class EmailMailBoxService : IEmailMailBoxService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        private readonly IMailHelper _mailHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.EmailMailBox> _repEmailMailBox;
        private readonly IRepositoryConfig<Database.EmailConfig> _repEmailConfig;
        private readonly IEmailAdminService _srvEmailAdmin;

        private string CreateUser = "admin";
        public EmailMailBoxService(
            IRepository<Database.EmailMailBox> repEmailMailBox,
            IRepositoryConfig<Database.EmailConfig> repEmailConfig,
            IRepository<Database.EmailAdmin> repEmailAdmin,
            IEmailAdminService srvEmailAdmin,
            IFileHelper fileHelper,
            ILoggerHelper logger,
            IMailHelper mailHelper)
        {
            _repEmailMailBox = repEmailMailBox;
            _repEmailConfig = repEmailConfig;
            _srvEmailAdmin = srvEmailAdmin;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repEmailConfig.GetAllConfigs();
            _mailHelper = mailHelper;
        }
        public Response<IPagedList<Database.EmailMailBox>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.EmailMailBox>> rs = new Response<IPagedList<Database.EmailMailBox>>();
            try
            {
                search.ToString();
                rs.Data = _repEmailMailBox.Query().Where(p => p.Deleted == false).Where(p => (String.IsNullOrEmpty(search.KeySearch) == true || p.EmailTittle.Contains(search.KeySearch) || p.EmailReceive.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex, Convert.ToInt32(_config[EmailAdminConstans.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response ReSendEmail(int id)
        {
            Response rs = new Response();
            try
            {
                var mailBox = _repEmailMailBox.Query().Where(s => s.Pid == id).FirstOrDefault();
                if (mailBox is not null)
                {
                    var emailAdmin = _srvEmailAdmin.GetById(mailBox.EmailAdminPid.Value).Data;
                    if(emailAdmin != null)
                    {
                        MailSettingModel mailSetting = new MailSettingModel(emailAdmin.Sender, emailAdmin.SmtpServer, emailAdmin.SmtpUser, emailAdmin.SmtpPassword, emailAdmin.Port, emailAdmin.EmailAddress);
                        _mailHelper.SendMails(mailBox.EmailReceive, mailBox.EmailTittle, mailBox.EmailContent, mailSetting);
                        rs.IsError = false;
                        rs.StatusCode = 200;
                        rs.Message = "ReSend mailbox success";
                        mailBox.IsFirstSendSuccess = true;
                        mailBox.CountSendSuccess += 1;
                    }
                    else
                    {
                        rs.IsError = true;
                        rs.StatusCode = 500;
                        rs.Message = "Email Admin does not exist!";
                    }
                    mailBox.CountReSend += 1;
                    Save(mailBox);
                }
                else
                {
                    rs.IsError = true;
                    rs.StatusCode = 404;
                    rs.Message = "Mailbox does not exist!";
                }
            }
            catch (Exception e)
            {
                rs.IsError = true;
                rs.StatusCode = 500;
                rs.Message = "Resend mailbox failed! \n" + e.Message;
                throw;
            }
            return rs;
        }
        public Response<EmailMailBox> GetById(int id)
        {
            Response<EmailMailBox> rs = new Response<EmailMailBox>();
            EmailMailBox detail = new EmailMailBox();
            try
            {

                detail = _repEmailMailBox.Query().Where(p => p.Pid == id).FirstOrDefault();
                //var file = _db.Email_Files.Where(p => p.EmailId == id).ToList();
                //if (file != null)
                //{
                //    detail.ListFiles = file;
                //}
                rs.IsError = false;

                rs.StatusCode = 200;
                rs.Data = detail;
                return rs;

            }
            catch (Exception ex)
            {
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, id.ToString());

            }
            return rs;
        }
        public Response<Database.EmailMailBox> Save(Database.EmailMailBox data)
        {
            var validator = new EmailMailBoxValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<EmailManagement.Database.EmailMailBox> rs = new Response<EmailManagement.Database.EmailMailBox>();
            using (var transaction = _repEmailMailBox.BeginTransaction())
            {
                try
                {
                    if(data.CreateUser == null)
                    {
                        data.CreateUser = CreateUser;
                    }
                    if (data.UpdateUser == null)
                    {
                        data.UpdateUser = CreateUser;
                    }
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;
                        data.CountSendSuccess = 1;
                        _repEmailMailBox.Add(data);

                        _repEmailMailBox.SaveChanges();
                    }
                    else
                    {
                        var model = _repEmailMailBox.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            //MailSetting;
                            model.EmailAdminPid = data.EmailAdminPid;
                            model.CountSendSuccess = data.CountSendSuccess;
                            model.CountReSend = data.CountReSend;
                            model.EmailReceive = data.EmailReceive;
                            model.ErrorDetail = data.ErrorDetail;
                            model.EmailCode = data.EmailCode;
                            model.EmailTittle = data.EmailTittle;
                            model.EmailContent = data.EmailContent;
                            _repEmailMailBox.SaveChanges();
                        }
                    }
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    rs.IsError = true;
                    rs.Message = ex.Message;
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, data.ToJson());

                }
            }
            rs.Data = data;
            return rs;

        }

        public Response Delete(List<int> ids)
        {

            Response rs = new Response();

            try
            {
                foreach (var id in ids)
                {
                    var model = _repEmailMailBox.Query().Where(p => p.Pid == id).FirstOrDefault();
                    model.Deleted = true;
                    _repEmailMailBox.SaveChanges();
                }


            }
            catch (Exception ex)
            {

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, ids.ToJson());

            }
            return rs;

        }


    }

}
