
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.DashBoard.Constants
{
    public static class DashBoardConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "DashBoard";
            public static string PathCommand = SystemInfo.VirtualFolder + "/commands/";

        }
        public class Config
        {
            public class Route
            {
                public static string PageIndex = SystemInfo.VirtualFolder + "/DashBoard";
                public static string PageEdit = SystemInfo.VirtualFolder + "/DashBoard/Edit";
                public static string ShortcutEdit = SystemInfo.VirtualFolder + "/DashBoard/Edit";
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

                public const string ViewPath = "/Views/DashBoard";
                public const string ViewPath_Shared = "/Views/Shared";



                public const string ShortCut = ViewPath_Shared +"/_ShortCut.cshtml";
                public const string ModalAdd_Shortcut = ViewPath_Shared + "/_ModalAddShortcut.cshtml";
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }
            public class Asset
            {

                public static string Image = SystemInfo.PathFileManager + "/DashBoard/img/";
                public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/DashBoard/img/";
            }

        }
    }
}
