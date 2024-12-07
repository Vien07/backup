
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.LayoutPage.Constants
{
    public static class QuickToolBarConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "QuickToolBar";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.LayoutPage/commands/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/QuickToolBar/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/QuickToolBar/Edit";
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

                public const string ApiUpdateQuickToolBar = TabName + "ApiUpdateQuickToolBar";
                public const string ApiUpdatePreviewQuickToolBar = TabName + "ApiUpdatePreviewQuickToolBar";
                public const string ApiRevertQuickToolBar = TabName + "ApiRevertQuickToolBar";
                public const string ApiPreviewPage = TabName + "ApiPreviewPage";
            }

        }
        public class StaticPath
        {

            public class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/QuickToolBar/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/QuickToolBar/Edit";
                public const string Edit = "/Views/PostsCategory/Edit.cshtml";
                public const string Edit_ModalQuickToolBarItemEdit = $"{ViewPath_Edit}/_ModalQuickToolBarItemEdit.cshtml";
                public const string Edit_Table = $"{ViewPath_Edit}/_Table.cshtml";
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";
                #endregion
                             public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";

            }
            public class Asset
            {

                public static string Image = SystemInfo.PathFileManager + "/QuickToolBar/img/";
                public static string ImageThumb = SystemInfo.PathFileManager + "/QuickToolBar/img/_thumb/";
            }

        }
    }
}

