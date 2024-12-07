
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

using Microsoft.EntityFrameworkCore;
using Steam.Infrastructure.Repository;

namespace Admin.EmailManagement.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.EmailTemplate> _repEmailTemplate;
        private readonly IRepositoryConfig<Database.EmailConfig> _repEmailConfig;
        public EmailTemplateService(
            IFileHelper fileHelper,
            IRepository<Database.EmailTemplate> repEmailTemplate,
            IRepositoryConfig<Database.EmailConfig> repEmailConfig,
            ILoggerHelper logger
            )
        {
            _repEmailConfig = repEmailConfig;
            _repEmailTemplate = repEmailTemplate;
            _logger = logger;
            _fileHelper = fileHelper;
            _config = _repEmailConfig.GetAllConfigs();

        }
        public Response<IPagedList<Database.EmailTemplate>> GetList(ParamSearch search)
        {
            Response<IPagedList<Database.EmailTemplate>> rs = new Response<IPagedList<Database.EmailTemplate>>();
            try
            {
                search.ToString();
                rs.Data = _repEmailTemplate.Query().Where(p=>p.Deleted==false).Where(p=>(String.IsNullOrEmpty(search.KeySearch)==true || p.Title.Contains(search.KeySearch)))
                    .OrderBy(p => p.Order).ThenBy(p => p.UpdateDate).ToList()
                    .ToPagedList(search.PageIndex,Convert.ToInt32(_config[EmailTemplateConstants.Config.Admin.PageSize]));
            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, search.ToJson());

            }
            return rs;
        }
        public Response<EmailTemplateDetail> GetById(int id)
        {
            Response<EmailTemplateDetail> rs = new Response<EmailTemplateDetail>();
            EmailTemplateDetail detail = new EmailTemplateDetail();
            try
            {

                detail.Detail = _repEmailTemplate.Query().Where(p => p.Pid == id).FirstOrDefault();
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
        public Response<Database.EmailTemplate> Save(Database.EmailTemplate data)
        {
            var validator = new EmailTemplateValidator();

            // Execute the validator
            ValidationResult results = validator.Validate(data);

            // Inspect any validation failures.
            bool success = results.IsValid;
            List<ValidationFailure> failures = results.Errors;
            Response<EmailManagement.Database.EmailTemplate> rs = new Response<EmailManagement.Database.EmailTemplate>();
            using (var transaction = _repEmailTemplate.BeginTransaction())
            {
                try
                {
                    if (data.Pid == 0)
                    {
                        data.Order = 0.9;

                        _repEmailTemplate.Add(data);

                        _repEmailTemplate.SaveChanges();
                    }
                    else
                    {
                        var model = _repEmailTemplate.Query().Where(p => p.Pid == data.Pid).FirstOrDefault();

                        if (model != null)
                        {
                            model.Title = data.Title;
                            model.Content = data.Content;
                            model.Header = data.Header;
                            model.Footer = data.Footer;
                            model.Key = data.Key;
                            model.EmailCode = data.EmailCode;
                            model.EmailName = data.EmailName;
                            model.EmailAdminPid = data.EmailAdminPid;
                            _repEmailTemplate.SaveChanges();

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
                    var model = _repEmailTemplate.Query().Where(p => p.Pid == id).FirstOrDefault();
                    model.Deleted = true;
                    _repEmailTemplate.Remove(model);
                    _repEmailTemplate.SaveChanges();
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
