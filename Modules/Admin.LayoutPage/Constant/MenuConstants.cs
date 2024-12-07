
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.LayoutPage.Constants
{
    public static class MenuConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "Menu";
            public static string PathCommand = SystemInfo.VirtualFolder + "/_content/Admin.LayoutPage/commands/";

        }
        public class Route
        {
            public static string PageIndex = SystemInfo.VirtualFolder + "/Menu/Index";
            public static string PageEdit = SystemInfo.VirtualFolder + "/Menu/Edit";
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
            }

        }
        public class StaticPath
        {

            public class PartialView
            {
                #region index
                public const string ViewPath_Index = "/Views/Menu/Index";
                public const string Index_Table = $"{ViewPath_Index}/_Table.cshtml";
                public const string Index_ModalConfig = $"{ViewPath_Index}/_ModalConfig.cshtml";
                public const string Index_TableTree = $"{ViewPath_Index}/_TableTree.cshtml";
                public const string Index_ModelEdit = $"{ViewPath_Index}/_ModalEdit.cshtml";

                #endregion
                #region Edit
                public const string ViewPath_Edit = "/Views/Menu/Edit";
                public const string Edit = "/Views/Menu/Edit.cshtml";
                #endregion

            }
            public class Asset
            {
                public const string Image = "/sample/img/";
                public const string ImageThumb = "/sample/img/_thumb/";
            }

        }
    }
}

