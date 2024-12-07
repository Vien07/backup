using Microsoft.EntityFrameworkCore;
using Admin.WebSetting.Database;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Admin.WebSetting.Constants;

namespace Admin.Course.Database
{
    public class WebsiteConfigurationContext : DbContext
    {
        public WebsiteConfigurationContext(DbContextOptions<WebsiteConfigurationContext> options) : base(options)
        {

        }

        public virtual DbSet<WebsiteConfiguration> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebsiteConfigurationContext).Assembly);

            //seed data
            SeedConfigurations(modelBuilder);
        }
        protected void SeedConfigurations(ModelBuilder modelBuilder)
        {
            int pid = 0;

            modelBuilder.Entity<WebsiteConfiguration>().HasData(
                new WebsiteConfiguration { Pid = ++pid, Key = "WebsiteName", Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "RootDomain", Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "ReCAPTCHASite", Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "ReCAPTCHASecret", Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "Recaptcha", Value = "", Group = "checkbox", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "Robots", Value = "", Group = "checkbox", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "Maintenance", Value = "", Group = "checkbox", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "Logo", Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "ogImage", Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "Favicon", Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "WebsiteDescription", Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "WebsiteMetaExtra", Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "WebsiteMeta", Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "ApiUpdateHomePageMeta", Value = "{host}/api/ApiMasterPage/UpdateHomePageMeta", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "ApiUpdateRobots", Value = "{host}/api/ApiMasterPage/UpdateRobots", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = "ApiGetUserOnline", Value = "{host}/api/ApiMasterPage/GetUserOnline", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = WebSettingConstants.ConfigName.TextRobotsOn, Value = "User-agent: * \r\nAllow: / \r\nSitemap: host/sitemap.xml \r\nDisallow: /s-admin/ \r\nDisallow: /*_escaped_fragment_", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = WebSettingConstants.ConfigName.TextRobotsOff, Value = "User-agent:* \n Disallow:/ ", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = WebSettingConstants.ConfigName.WebsiteStyleVariables, Value = " :root {}", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = WebSettingConstants.ConfigName.FontSize, Value = "100", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = WebSettingConstants.ConfigName.WebsiteCustomStyle, Value = "", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = WebSettingConstants.ConfigName.ApiUpdateVariableStyle, Value = "{host}/api/ApiMasterPage/UpdateVariableStyle", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = WebSettingConstants.ConfigName.ApiUpdateWebconfigValue, Value = "{host}/api/ApiMasterPage/UpdateWebconfigValue", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = WebSettingConstants.ConfigName.ApiUpdateWebsiteCustomStyle, Value = "{host}/api/ApiMasterPage/UpdateWebsiteCustomStyle", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = WebSettingConstants.ConfigName.ApiGetTrafficReport, Value = "{host}/api/ApiMasterPage/GetTrafficReport", Group = "text", CreateUser = "admin", UpdateUser = "admin" },
                new WebsiteConfiguration { Pid = ++pid, Key = WebSettingConstants.ConfigName.ApiGetTraffic, Value = "{host}/api/ApiMasterPage/GetTraffic", Group = "text", CreateUser = "admin", UpdateUser = "admin" }
                );


        }

    }
}