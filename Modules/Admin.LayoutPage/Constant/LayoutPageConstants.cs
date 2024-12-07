
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.LayoutPage.Constants
{
    public static class LayoutPageConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "LayoutPage";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.LayoutPage/commands/";

        }
        public class Config
        {
            public class Route
            {
                public static string PageIndex = SystemInfo.VirtualFolder + "/LayoutPage";
                public static string PageEdit = SystemInfo.VirtualFolder + "/LayoutPage/Edit";
            }
            public class Admin
            {
                public const string TabName = "Admin_";

                public const string PageSize = TabName + "PageSize";
            }
            public class Website
            {
                public const string TabName = "Website_";

                public const string AlwaysShowTop = TabName + "AlwaysShowTop";

                public const string ApiUpdateLayoutPage = TabName + "ApiUpdateLayoutPage";
                public const string ApiRevertLayoutPage = TabName + "ApiRevertLayoutPage";
            }

        }
        public class StaticPath
        {

            public class PartialView
            {

                public const string ViewPath = "/Views/LayoutPage";
                public const string PartialModalsPath = ViewPath + "/Modals";
                public const string PartialTablesPath = ViewPath + "/Tables";
                public const string PartialTabsPath = ViewPath + "/Tabs";

                public const string Index_Table = PartialTablesPath + "/Index" + "/_Table.cshtml";
                public const string Edit_Table = PartialTablesPath + "/Edit" + "/_Table.cshtml";
                public const string Edit_TabSEO = PartialTabsPath + "/Edit" + "/_TabSEO.cshtml";
                public const string Edit_TabContent = PartialTabsPath + "/Edit" + "/_TabContent.cshtml";
                public const string Index_ModalConfig = PartialModalsPath + "/Index" + "/_ModalConfig.cshtml";
                public const string Edit_ModalFooterItemEdit = PartialModalsPath + "/Edit" + "/_ModalFooterItemEdit.cshtml";
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";

            }
            public class Asset
            {
                public static string Image = SystemInfo.PathFileManager + "/LayoutPage/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/LayoutPage/img/";

            }

        }
    }
}

