using Steam.Core.Base.Constant;

namespace Admin.WebSetting.Constants
{
    public static class WebSettingConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "WebSetting";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.WebSetting/commands/";

        }
        public class ConfigName
        {
            public const string WebsiteName = "WebsiteName";
            public const string RootDomain = "RootDomain";
            public const string Logo = "Logo";
            public const string ogImage = "ogImage";
            public const string Favicon = "Favicon";
            public const string ReCAPTCHASite = "ReCAPTCHASite";
            public const string ReCAPTCHASecret = "ReCAPTCHASecret";
            public const string Recaptcha = "Recaptcha";
            public const string Robots = "Robots";
            public const string Maintenance = "Maintenance";
            public const string WebsiteDescription = "WebsiteDescription";
            public const string WebsiteMetaExtra = "WebsiteMetaExtra";
            public const string WebsiteMeta = "WebsiteMeta";
            public const string WebsiteStyleVariables = "WebsiteStyleVariables";
            public const string FontSize = "FontSize";
            public const string ApiUpdateRobots = "ApiUpdateRobots";
            public const string TextRobotsOn = "TextRobotsOn";
            public const string TextRobotsOff = "TextRobotsOff";
            public const string ApiUpdateHomePageMeta = "ApiUpdateHomePageMeta";
            public const string ApiGetUserOnline = "ApiGetUserOnline";
            public const string WebsiteCustomStyle = "WebsiteCustomStyle";
            public const string ApiUpdateVariableStyle = "WebsiteCustomStyle";
            public const string ApiUpdateWebconfigValue = "ApiUpdateWebconfigValue";
            public const string ApiUpdateWebsiteCustomStyle = "ApiUpdateWebsiteCustomStyle";
            public const string ApiGetTrafficReport = "ApiGetTrafficReport";
            public const string ApiGetTraffic = "ApiGetTraffic";
        }

        public class StaticPath
        {

            public class PartialView
            {

                public const string ViewPath = "/Views/WebSetting";
                public const string PartialModalsPath = ViewPath + "/Modals";
                public const string PartialTablesPath = ViewPath + "/Tables";
                public const string PartialTabsPath = ViewPath + "/Tabs";

                public const string Table = PartialTablesPath + "/_Table.cshtml";
                public const string TabInfo = PartialTabsPath + "/_TabInfo.cshtml";
                public const string TabImages = PartialTabsPath + "/_TabImages.cshtml";
                public const string TabActions = PartialTabsPath + "/_TabAction.cshtml";
                public const string TabStyle = PartialTabsPath + "/_TabStyle.cshtml";
                public const string TabApi = PartialTabsPath + "/_TabApi.cshtml";
                public const string ModalConfig = PartialModalsPath + "/_ModalConfig.cshtml";
                public const string ModalFooterItemEdit = PartialModalsPath + "/Edit" + "/_ModalFooterItemEdit.cshtml";
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";

            }
            public class Asset
            {

                public static string Image = SystemInfo.PathFileManager + "/WebSetting/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/WebSetting/img/";
                public static string ImageDefault = SystemInfo.PathFileManager + "/Website/img/";
                public static string FontsAddon = SystemInfo.PathFileManager + "/Website/fonts/";
            }

        }
    }
}
