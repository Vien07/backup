using Microsoft.Extensions.DependencyInjection;
using Steam.Core.Utilities.STeamHelper;

namespace Steam.Core.Utilities
{
    public static class Initialization
    {
        public static void AddModuleCommon(this IServiceCollection services)
        {
            services.AddScoped<ILoggerHelper, LoggerHelper>();
            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IViewRendererHelper, ViewRendererHelper>();
            services.AddScoped<IMailHelper, MailHelper>();
            services.AddScoped<IMetaHelper, MetaHelper>();
            services.AddScoped<IRandomString, RamdomString>();
            services.AddScoped<IRestHelper, RestHelper>();
        }
    }
}
