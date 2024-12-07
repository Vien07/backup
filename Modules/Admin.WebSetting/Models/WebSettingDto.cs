using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.WebSetting.Models
{
    public class WebSettingDto
    {
        public string WebsiteName { get; set; }
        public string RootDomain { get; set; }
        public string MinWidth { get; set; }
        public string MaxWidth { get; set; }
        public IFormFile Logo { get; set; }
        public IFormFile Favicon { get; set; }
        public IFormFile ogImage { get; set; }
        public string Recaptcha { get; set; }
        public string Robots { get; set; }
        public string Maintenance { get; set; }
        public string ReCAPTCHASite { get; set; }
        public string ReCAPTCHASecret { get; set; }
        public string WebsiteDescription { get; set; }
        public string WebsiteMetaExtra { get; set; }
        public string WebsiteMeta { get; set; }
        public string ApiUpdateRobots { get; set; }
        public string ApiUpdateHomePageMeta { get; set; }
        public string ApiGetUserOnline { get; set; }
        public string TextRobotsOff { get; set; }
        public string TextRobotsOn { get; set; }
        public string? WebsiteStyleVariables { get; set; }
        public string? WebsiteCustomStyle { get; set; }
        public string? ApiUpdateWebconfigValue { get; set; }
        public string? ApiGetTrafficReport { get; set; }
        public string? ApiGetTraffic { get; set; }
        public string? FontSize { get; set; }


    }
}
