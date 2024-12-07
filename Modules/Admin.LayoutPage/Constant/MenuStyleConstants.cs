
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.LayoutPage.Constants
{
    public static class MenuStyleConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "Menu";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.LayoutPage/commands/";

        }
        public class Config
        {
            public class Route
            {
                public static string PageIndex = SystemInfo.VirtualFolder + "/MenuStyle";
                public static string PageEdit = SystemInfo.VirtualFolder + "/MenuStyle/Edit";
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
            }

        }
        public class StaticPath
        {

            public class PartialView
            {

                public const string ViewPath = "/Views/MenuStyle";
                public const string PartialModalsPath = ViewPath + "/Modals";
                public const string PartialTablesPath = ViewPath + "/Tables";
                public const string PartialTabsPath = ViewPath + "/Tabs";

                public const string Index_Table = PartialTablesPath + "/Index" + "/_Table.cshtml";
                public const string Edit_Table = PartialTablesPath + "/Edit" + "/_Table.cshtml";
                public const string Edit_TabSEO = PartialTabsPath + "/Edit" + "/_TabSEO.cshtml";
                public const string Edit_TabContent = PartialTabsPath + "/Edit" + "/_TabContent.cshtml";
                public const string Index_ModalConfig = PartialModalsPath + "/Index" + "/_ModalConfig.cshtml";
                public const string Edit_ModalMenuItemEdit = PartialModalsPath + "/Edit" + "/_ModalMenuItemEdit.cshtml";
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";

            }
            public class Asset
            {
                public const string Image = "/sample/img/";
                public const string ImageThumb = "/sample/img/_thumb/";
            }

        }
    }
}

