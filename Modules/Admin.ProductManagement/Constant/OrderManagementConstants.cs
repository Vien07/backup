
using Steam.Core.Base.Constant;


namespace Admin.ProductManagement.Constants
{
    public static class OrderManagementConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "OrderManagement";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.ProductManagement/commands/";

        }
        public class Config
        {
            public class Route
            {
                public static string PageIndex = SystemInfo.VirtualFolder + "/OrderManagement";
                public static string PageEdit = SystemInfo.VirtualFolder + "/OrderManagement/Edit";
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

                public const string ViewPath = "/Views/OrderManagement";
                public const string PartialModalsPath = ViewPath + "/Modals";
                public const string PartialTablesPath = ViewPath + "/Tables";
                public const string PartialTabsPath = ViewPath + "/Tabs";

                public const string Index_Table = PartialTablesPath + "/Index" + "/_Table.cshtml";
                public const string Edit_TabContent = PartialTabsPath + "/Edit" + "/_TabContent.cshtml";
                public const string Index_ModalConfig = PartialModalsPath + "/Index" + "/_ModalConfig.cshtml";
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }
            public class Asset
            {

                public static string Image = SystemInfo.PathFileManager + "/OrderManagement/img/";
                public static string ImageThumb = SystemInfo.PathFileManager + "/OrderManagement/img/_thumb/";
            }

        }
    }
}
