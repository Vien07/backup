
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.LayoutPage.Constants
{
    public static class HeaderPageConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "HeaderPage";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.LayoutPage/commands/";

        }
        public class Config
        {
            public class Route
            {
                public static string PageIndex = SystemInfo.VirtualFolder + "/HeaderPage";
                public static string PageEdit = SystemInfo.VirtualFolder + "/HeaderPage/Edit";
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
                public const string AlwaysShowTop = TabName + "AlwaysShowTop";

                public const string ApiUpdateHeader = TabName + "ApiUpdateHeader";
                public const string ApiUpdatePreviewHeader = TabName + "ApiUpdatePreviewHeader";
                public const string ApiRevertHeader = TabName + "ApiRevertHeader";
                public const string ApiPreviewPage = TabName + "ApiPreviewPage";
            }

        }
        public class StaticPath
        {

            public class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/HeaderPage/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/HeaderPage/Edit";
                public const string Edit = "/Views/HeaderPage/Edit.cshtml";
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";
                #endregion
 


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
