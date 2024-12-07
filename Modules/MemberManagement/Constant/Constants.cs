
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.MemberManagement.Constants
{
    public static class ModuleInfo
    {
        public static string ModuleName = "MemberManagement";
        public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.MemberManagement/commands/";

    }

    public class Config
    {
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder+"/MemberManagement";
            public static string PageEdit = SystemInfo.VirtualFolder+"/MemberManagement/Edit";
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
    public  class StaticPath
    {

        public class PartialView
        {
            #region index
            const string ViewPath_Index = "/Views/MemberManagement/Index";
            public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
            public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
            #endregion
            #region Edit
            public const string ViewPath_Edit = "/Views/MemberManagement/Edit";
            public const string Edit = "/Views/MemberManagement/Edit.cshtml";
            #endregion

            public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
        }
        public class Asset
        {
            public const string Image = SystemInfo.PathFileManager+ "/MemberManagement/img/";
            public const string ImageThumb = SystemInfo.ThumbsBasePathFileManager+"/MemberManagement/img/";
        }

    }
}
