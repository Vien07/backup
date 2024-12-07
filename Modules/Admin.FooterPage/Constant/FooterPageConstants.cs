
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.FooterPage.Constants
{
    public static class FooterPageConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "FooterPage";
            public static string PathCommand = "/_content/Admin.FooterPage/commands/";

        }
        public class Config
        {
            public class Route
            {
                public const string PageIndex = "/FooterPage";
                public const string PageEdit = "/FooterPage/Edit";
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

                public const string ViewPath = "/Views/FooterPage";
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
                public const string Image = "/footerpage/img/";
                public const string ImageThumb = "/footerpage/img/_thumb/";
            }

        }
    }
}

