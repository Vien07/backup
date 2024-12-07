
using Microsoft.AspNetCore.Http;

namespace Admin.WebSetting.Models
{
    public class WebSettingViewModelModelEdit : WebSettingDto
    {
        public string isReCAPTCHA { get; set; }
        public string AllowRobots { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string FileStatus { get; set; }
        public string FilePath { get; set; }
        public string isNewCheckBox { get; set; }
        public string FileStatus_Logo { get; set; }
        public string FilePath_Logo { get; set; }
        public string FileStatus_ogImage { get; set; }
        public string FilePath_ogImage { get; set; }
        public string FileStatus_Favicon { get; set; }
        public string FilePath_Favicon { get; set; }
        public int TypeSave { get; set; }
        public WebSettingDto GetDatabaseModel()
        {
            WebSettingDto rs = new WebSettingDto();
            if (this.Recaptcha == null)
            {
                rs.Recaptcha = "off";
            }
            if (this.Robots == null)
            {
                rs.Robots = "off";
            }
            rs.WebsiteName = this.WebsiteName;
            rs.RootDomain = this.RootDomain;
            rs.MinWidth = this.MinWidth;
            rs.MaxWidth = this.MaxWidth;
            rs.Logo = this.Logo;
            rs.Favicon = this.Favicon;
            rs.ogImage = this.ogImage;
            rs.Recaptcha = this.Recaptcha;
            rs.Robots = this.Robots;
            rs.Maintenance = this.Maintenance;
            rs.ReCAPTCHASite = this.ReCAPTCHASite;
            rs.ReCAPTCHASecret = this.ReCAPTCHASecret;
            rs.WebsiteDescription = this.WebsiteDescription;
            rs.WebsiteMetaExtra = this.WebsiteMetaExtra;
            rs.WebsiteMeta = this.WebsiteMeta;
            rs.ApiUpdateRobots = this.ApiUpdateRobots;
            rs.ApiUpdateHomePageMeta = this.ApiUpdateHomePageMeta;
            rs.ApiGetUserOnline = this.ApiGetUserOnline;
            rs.TextRobotsOff = this.TextRobotsOff;
            rs.TextRobotsOn = this.TextRobotsOn;
            rs.WebsiteStyleVariables = this.WebsiteStyleVariables;
            rs.ApiUpdateWebconfigValue = this.ApiUpdateWebconfigValue;
            rs.ApiGetTrafficReport = this.ApiGetTrafficReport;
            rs.ApiGetTraffic = this.ApiGetTraffic;
            rs.FontSize = this.FontSize;
            return rs;
        }

    }


}
