
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.LayoutPage.Constants
{
    public static class FooterPageConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "FooterPage";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.LayoutPage/commands/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/FooterPage";
            public static string PageEdit = SystemInfo.VirtualFolder + "/FooterPage/Edit";
        }
        public class Config
        {
 
            public class Admin
            {
                public const string TabName = "Admin_";

                public const string PageSize = TabName + "PageSize";
            }
            public class Website
            {
                public const string TabName = "Website_";

                public const string AlwaysShowTop = TabName + "AlwaysShowTop";

                public const string ApiUpdateFooterPage = TabName + "ApiUpdateFooterPage";
                public const string ApiUpdatePreviewFooterPage = TabName + "ApiUpdatePreviewFooterPage";
                public const string ApiRevertFooterPage = TabName + "ApiRevertFooterPage";
                public const string ApiPreviewPage = TabName + "ApiPreviewPage";
            }

        }
        public class StaticPath
        {

            public class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/FooterPage/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                public const string Index_TableTree = $"{ViewPath_Index}/_TableTree.cshtml";
                public const string Index_ModelEdit = $"{ViewPath_Index}/_ModalEdit.cshtml";

                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/FooterPage/Edit";
                public const string Edit = "/Views/FooterPage/Edit.cshtml";
                public const string Edit_ModalFooterItemEdit = $"{ViewPath_Edit}/_ModalFooterItemEdit.cshtml";
                public const string Edit_TabContent = $"{ViewPath_Edit}/_TabContent.cshtml";
                public const string Edit_Table = $"{ViewPath_Edit}/_Table.cshtml";

                #endregion
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";

            }
            public class Asset
            {
                public const string Image = "/footerpage/img/";
                public const string ImageThumb = "/footerpage/img/_thumb/";
            }

        }
    }
}

