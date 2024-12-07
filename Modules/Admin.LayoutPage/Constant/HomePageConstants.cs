
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.LayoutPage.Constants
{
    public static class HomePageConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "HomePage";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.LayoutPage/commands/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/HomePage/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/HomePage/Edit";
        }
        public class Config
        {

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

                public const string ApiUpdateHomePage = TabName + "ApiUpdateHomePage";
                public const string ApiUpdatePreviewHomePage = TabName + "ApiUpdatePreviewHomePage";
                public const string ApiRevertHomePage = TabName + "ApiRevertHomePage";
                public const string ApiPreviewPage = TabName + "ApiPreviewPage";
            }

        }
        public class StaticPath
        {

            public class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/HomePage/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                public const string Index_TableTree = $"{ViewPath_Index}/_TableTree.cshtml";
                public const string Index_ModelEdit = $"{ViewPath_Index}/_ModalEdit.cshtml";

                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/HomePage/Edit";
                public const string Edit = "/Views/HomePage/Edit.cshtml";
                #endregion


                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }


        }
    }
}
