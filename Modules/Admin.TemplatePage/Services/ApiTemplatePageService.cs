
using Microsoft.AspNetCore.Http;
using Admin.TemplatePage.Database;
using Admin.TemplatePage.Models;
using System.Reflection;
using X.PagedList;
using FluentValidation.Results;
using Admin.TemplatePage.Constants;
using Steam.Core.Utilities.STeamHelper;
using Steam.Core.Base.Models;
using Steam.Core.Common.SteamString;
using Steam.Core.Utilities.SteamModels;
using Steam.Core.Base.Constant;
using Admin.SEO;
using Steam.Infrastructure.Repository;
using Admin.SEO.Repository;
using Admin.SEO.Services;

namespace Admin.TemplatePage.Services
{
    public class ApiTemplatePageService : IApiTemplatePageService
    {
        private ILoggerHelper _logger;
        Dictionary<string, string> _CONFIG;
        private ISEOService _srvSEO;
        private readonly IRepository<Database.TemplatePage> _repTemplatePage;
        private readonly IRepositoryConfig<Database.TemplatePageConfig> _repTemplatePageConfig;
        public ApiTemplatePageService(
            IRepository<Database.TemplatePage> repTemplatePage,
            IRepositoryConfig<Database.TemplatePageConfig> repTemplatePageConfig,
            ISEOService srvSEO,
            ILoggerHelper logger)
        {
            _srvSEO = srvSEO;
            _repTemplatePage= repTemplatePage;
            _repTemplatePageConfig = repTemplatePageConfig;
            _logger = logger;
            _CONFIG = _repTemplatePageConfig.GetAllConfigs();


        }

        public Response<TemplatePageMeta> GetDefaultMeta(string controller, string action)
        {
            var page = string.Format("/{0}/{1}", controller, action);

            Response<TemplatePageMeta> rs = new Response<TemplatePageMeta>();
            TemplatePageMeta detail = new TemplatePageMeta();
            try
            {
                var tempPage = _repTemplatePage.Query().Where(p => p.Url == page).FirstOrDefault();
                if (tempPage != null)
                {
                    var seo = _srvSEO.GetSEO(tempPage.Pid, TemplatePageConstants.ModuleInfo.ModuleCode);
                    if (seo != null)
                    {
                        detail.Meta = (seo.Meta ?? "").ToRemoveBreakSympol();

                    }
                    
                }
                rs.IsError = false;

                rs.StatusCode = 200;
                rs.Data = detail;
                return rs;

            }
            catch (Exception ex)
            {
                detail.Meta = "";
                rs.StatusCode = 500;

                rs.IsError = true;
                rs.Message = ex.Message;
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name, ex.Message, page.ToString());

            }
            return rs;
        }


    }

}
