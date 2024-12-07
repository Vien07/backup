
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.HomePage.Constants
{
    public static class HomePageConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "HomePage";
            public static string PathCommand = "/_content/Admin.HomePage/commands/";

        }
        public class Config
        {
            public class Route
            {
                public const string PageIndex = "/HomePage";
                public const string PageEdit = "/HomePage/Edit";
            }
            public class Admin
            {
                public const string TabName = "Admin_";

                public const string PageSize = TabName + "PageSize";
                public const string ThumbWidth = TabName + "ThumbWidth";
                public const string ThumbHeight = TabName + "ThumbHeight";
                public const string MaxHeight = TabName + "MaxHeight";
                public const string MaxWidth = TabName + "MaxWidth";
                public const string Config1 = TabName + "Config1";
            }
            public class Website
            {
                public const string TabName = "Website_";

                public const string PreSlug = TabName + "PreSlug";
                public const string PageSize = TabName + "PageSize";
            }

        }
        public class StaticPath
        {

            public class PartialView
            {

                public const string ViewPath = "/Views/HomePage";
                public const string PartialModalsPath = ViewPath + "/Modals";
                public const string PartialTablesPath = ViewPath + "/Tables";
                public const string PartialTabsPath = ViewPath + "/Tabs";

                public const string Index_Table = PartialTablesPath + "/Index" + "/_Table.cshtml";
                public const string Edit_TabSEO = PartialTabsPath + "/Edit" + "/_TabSEO.cshtml";
                public const string Edit_TabCategory = PartialTabsPath + "/Edit" + "/_TabCategory.cshtml";
                public const string Edit_TabSetting = PartialTabsPath + "/Edit" + "/_TabSetting.cshtml";
                public const string Edit_TabImages = PartialTabsPath + "/Edit" + "/_TabImages.cshtml";
                public const string Edit_TabContent = PartialTabsPath + "/Edit" + "/_TabContent.cshtml";
                public const string Index_ModalConfig = PartialModalsPath + "/Index" + "/_ModalConfig.cshtml";
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }


        }
    }
}
