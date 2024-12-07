using Admin.EmailManagement.Database;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common;
using Steam.Infrastructure.Repository;

namespace Admin.EmailManagement.Services
{
    public class ApiEmailManagementService : IApiEmailManagementService
    {
        private ILoggerHelper _logger;
        IFileHelper _fileHelper;
        Dictionary<string, string> _config;
        private readonly IRepository<Database.EmailTemplate> _repEmailTemplate;
        public ApiEmailManagementService(
            IRepository<Database.EmailTemplate> repEmailTemplate,
            ILoggerHelper logger)
        {
            _repEmailTemplate = repEmailTemplate;
            _logger = logger;

        }



        public Response<Api.Models.Response.GetEmailConfig> GetEmailConfig(string code)
        {
            Response<Api.Models.Response.GetEmailConfig> rs = new Response<Api.Models.Response.GetEmailConfig>();
            try
            {
                var emailTemplate = _repEmailTemplate.Query().Where(p => p.EmailCode == code).FirstOrDefault();
                if (emailTemplate != null)
                {
                    var emailConfig = _repEmailTemplate.Query().Where(p => p.Pid == emailTemplate.EmailAdminPid).FirstOrDefault();


                    Api.Models.Response.GetEmailConfig email = emailConfig.DeepClone<Api.Models.Response.GetEmailConfig>();
                    rs.Data = email;
                    return rs;
                }


            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.ToString();
            }
            return rs;
        }  
        public Response<Api.Models.Response.GetEmailTemplate> GetEmailTemplate(string code)
        {
            Response<Api.Models.Response.GetEmailTemplate> rs = new Response<Api.Models.Response.GetEmailTemplate>();
            try
            {
                var emailTemplate = _repEmailTemplate.Query().Where(p => p.EmailCode == code).FirstOrDefault();

                Api.Models.Response.GetEmailTemplate email = emailTemplate.DeepClone<Api.Models.Response.GetEmailTemplate>();
                rs.Data = email;
                return rs;


            }
            catch (Exception ex)
            {
                rs.IsError = true;
                rs.Message = ex.ToString();
            }
            return rs;
        }

    }

}
