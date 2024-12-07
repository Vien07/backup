
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.HeaderPage.Constants
{
    public static class HeaderPageConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "HeaderPage";
            public static string PathCommand = "/_content/Admin.HeaderPage/commands/";

        }
        public class Config
        {
            public class Route
            {
                public const string PageIndex = "/HeaderPage";
                public const string PageEdit = "/HeaderPage/Edit";
            }
            public class Admin
            {
                public const string TabName = "Admin_";

                public const string PageSize = TabName + "PageSize";
                public const string MaxHeight = TabName + "MaxHeight";
                public const string MaxWidth = TabName + "MaxWidth";
                public const string Config1 = TabName + "Config1";
            }
            public class Website
            {
                public const string TabName = "Website_";

                public const string ApiUpdateHeader = TabName + "ApiUpdateHeader";
                public const string ApiRevertHeader = TabName + "ApiRevertHeader";
            }

        }
        public class StaticPath
        {

            public class PartialView
            {

                public const string ViewPath = "/Views/HeaderPage";
                public const string PartialModalsPath = ViewPath + "/Modals";
                public const string PartialTablesPath = ViewPath + "/Tables";
                public const string PartialTabsPath = ViewPath + "/Tabs";

                public const string Index_Table = PartialTablesPath + "/Index" + "/_Table.cshtml";
                public const string Edit_TabSEO = PartialTabsPath + "/Edit" + "/_TabSEO.cshtml";
                public const string Edit_TabContent = PartialTabsPath + "/Edit" + "/_TabContent.cshtml";
                public const string Index_ModalConfig = PartialModalsPath + "/Index" + "/_ModalConfig.cshtml";
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }
            public static class ExtenalViews
            {

                public static class SEO
                {
                    public const string ExtenalViewslUrl = "/Views/SEOIntegrate";
                    public const string TabSEO = ExtenalViewslUrl + "/TabSEO.cshtml";
                }


            }

        }
    }
}
