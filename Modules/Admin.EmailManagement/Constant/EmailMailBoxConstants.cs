
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.EmailManagement.Constants
{
    public static class EmailMailBoxConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "EmailMailBox";
            public static string PathCommand = SystemInfo.VirtualFolder+ "/_content/Admin.EmailManagement/commands/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/EmailMailBox/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/EmailMailBox/Edit";
        }
        public class Config
        {

            public class Admin
            {
                public const string TabName = "Admin_";

                public const string PageSize = TabName + "PageSize";
                public const string MaxHeight = TabName + "MaxHeight";
                public const string MaxWidth = TabName + "MaxWidth";
                public const string Config1 = TabName + "Config1";
            }


        }
        public class StaticPath
        {

            public class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/EmailMailBox/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/EmailMailBox/Edit";
                public const string Edit = "/Views/EmailMailBox/Edit.cshtml";
                public const string Edit_TabEmail = $"{ViewPath_Edit}/_TabEmail.cshtml";
                public const string Edit_TabTemplate = $"{ViewPath_Edit}/Edit_TabTemplate.cshtml";
                #endregion

                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
            }

        }
    }
}
