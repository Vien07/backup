
using Steam.Core.Base.Constant;


namespace Admin.Authorization.Constants
{
    public static class LogManagementConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "LogManagement";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.Authorization/commands/";

        }
        public class Config
        {
            public class Route
            {
                public static string PageIndex = SystemInfo.VirtualFolder + "/LogManagement";
                public static string PageEdit = SystemInfo.VirtualFolder + "/LogManagement/Edit";
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
                #region index
                const string ViewPath_Index = "/Views/LogManagement/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/LogManagement/Edit";
                public const string Edit = "/Views/LogManagement/Edit.cshtml";
                #endregion
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }
            public class Asset
            {

                public static string Image = SystemInfo.PathFileManager + "/LogManagement/img/";
                public static string ImageThumb = SystemInfo.PathFileManager + "/LogManagement/img/_thumb/";
            }

        }
    }
}
