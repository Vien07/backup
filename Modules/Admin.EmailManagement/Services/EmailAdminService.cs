
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

using Steam.Infrastructure.Repository;

namespace Admin.EmailManagement.Services
{
    public class EmailAdminService : IEmailAdminService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.EmailAdmin> _repEmailAdmin;
        private readonly IRepositoryConfig<Database.EmailConfig> _repEmailConfig;
        public EmailAdminService(
           IRepository<Database.EmailAdmin> repEmailAdmin,
           IRepositoryConfig<Database.EmailConfig> repEmailConfig,
            IFileHelper fileHelper,
            ILoggerHelper logger)
        {
            _repEmailAdmin = repEmailAdmin;
            _repEmailConfig = repEmailConfig;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repEmailConfig.GetAllConfigs();



        }
        public Response<IPagedList<Database.EmailAdmin>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.EmailAdmin>> rs = new Response<IPagedList<Database.EmailAdmin>>();
            try
            {
                search.ToString();
                rs.Data = _repEmailAdmin.Query().Where(p=>p.Deleted==false).Where(p=>(String.IsNullOrEmpty(search.KeySearch)==true || p.EmailAddress.Contains(search.KeySearch) || p.Sender.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_config[EmailAdminConstans.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<EmailAdmin> GetById(long id)
        {
            Response<EmailAdmin> rs = new Response<EmailAdmin>();
            EmailAdmin detail = new EmailAdmin();
            try
            {

                detail = _repEmailAdmin.Query().Where(p => p.Pid == id).FirstOrDefault();
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
        public Response<Database.EmailAdmin> Save(Database.EmailAdmin data)
        {
            var validator = new EmailAdminValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<EmailManagement.Database.EmailAdmin> rs = new Response<EmailManagement.Database.EmailAdmin>();
            using (var transaction = _repEmailAdmin.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repEmailAdmin.Add(data);

                        _repEmailAdmin.SaveChanges();
                    }
                    else
                    {
                        var model = _repEmailAdmin.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.EmailAddress = data.EmailAddress;
                            model.Sender = data.Sender;
                            model.SmtpServer = data.SmtpServer;
                            model.SmtpUser = data.SmtpUser;
                            model.SmtpPassword = data.SmtpPassword;
                            model.Port = data.Port;
                            _repEmailAdmin.SaveChanges();

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
                    var model = _repEmailAdmin.Query().Where(p => p.Pid == id).FirstOrDefault();
                    model.Deleted = true;
                    _repEmailAdmin.Remove(model);
                    _repEmailAdmin.SaveChanges();
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
