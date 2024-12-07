
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Authorization.Constants
{
    public static class ModuleInfo
    {
        public static string ModuleName = "Authorization";
        public static string PathCommand = SystemInfo.VirtualFolder +"/_content/Admin.Authorization/commands/";

    }
    public class Config
    {
        public class Route
        {
            // ModuleRole Route
            public static string ModuleRolePageIndex = SystemInfo.VirtualFolder + "/ModuleRole/Index";
            public static string ModuleRolePageEdit = SystemInfo.VirtualFolder + "/ModuleRole/Edit";

            // GroupRole Route
            public static string GroupRolePageIndex = SystemInfo.VirtualFolder + "/GroupRole/Index";
            public static string GroupRolePageEdit = SystemInfo.VirtualFolder + "/GroupRole/Edit";

            // Account Route
            public static string AccountPageIndex = SystemInfo.VirtualFolder + "/Account/Index";
            public static string AccountPageEdit = SystemInfo.VirtualFolder + "/Account/Edit";
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
            // ModuleRole PartialView
            public const string _PartialUrlModuleRole = "/Views/ModuleRole/_Partial/";
            public const string _PartialModelModuleRole = "_PartialModalModuleRole.cshtml";
            public const string _PartialTableModuleRole = "_PartialTable.cshtml";
            public const string _PartialTableModuleRoleChild = "_PartialTableModuleRole.cshtml";
            public const string _PartialModalConfigModuleRole = "_PartialModalConfig.cshtml";
            public const string _PartialPageTitleModuleRole = "/Views/Shared/_PageTitle.cshtml";

            // GroupRole PartialView
            public const string _PartialUrlGroupRole = "/Views/GroupRole/_Partial/";
            public const string _PartialModelGroupRole = "_PartialModal.cshtml";
            public const string _PartialTableGroupRole = "_PartialTable.cshtml";
            public const string _PartialTableListModuleRole = "_PartialTableListModuleRole.cshtml";
            public const string _PartialModalConfigGroupRole = "_PartialModalConfig.cshtml";
            public const string _PartialPageTitleGroupRole = "/Views/Shared/_PageTitle.cshtml";

            // Account PartialView

            #region index
            const string ViewPath_Index = "/Views/Account/Index";
            public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
            public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
            #endregion
            #region Edit
            public const string ViewPath_Edit = "/Views/Account/Edit";
            public const string Edit = "/Views/Account/Edit.cshtml";
            #endregion
            public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";

        }
        public class PartialViewModuleRole
        {

            #region index
            const string ViewPath_Index = "/Views/ModuleRole/Index";
            public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
            public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
            #endregion
            #region Edit
            public const string ViewPath_Edit = "/Views/ModuleRole/Edit";
            public const string Edit = "/Views/ModuleRole/Edit.cshtml";
            #endregion
            public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
        }
        public class PartialViewGroupRole
        {


            // Account PartialView
            public const string _PartialTableListModuleRole = "_PartialTableListModuleRole.cshtml";

            #region index
            const string ViewPath_Index = "/Views/GroupRole/Index";
            public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
            public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
            #endregion
            #region Edit
            public const string ViewPath_Edit = "/Views/GroupRole/Edit";
            public const string Edit = "/Views/GroupRole/Edit.cshtml";
            #endregion
            public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";

        }
        public static class Asset
        {
            public static string Image = SystemInfo.PathFileManager + "/Authorization/img/";
            public static string ImageThumb = SystemInfo.ThumbsBasePathFileManager + "/Authorization/img/";
        }


    }
}
