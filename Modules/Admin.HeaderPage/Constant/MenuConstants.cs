
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.HeaderPage.Constants
{
    public static class MenuConstants
    {
        public static class ModuleInfo
        {
            public static string ModuleName = "Menu";
            public static string PathCommand = "/_content/Admin.HeaderPage/commands/";

        }
        public class Config
        {
            public class Route
            {
                public const string PageIndex = "/Menu";
                public const string PageEdit = "/Menu/Edit";
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

                public const string _PartialUrl = "/Views/Menu/_Partial/";
                public const string _ModalUrl = "/Views/Menu/Modals/Index/";
                public const string _PartialModel = "_PartialModal.cshtml";
                public const string _PartialTable = "_PartialTable.cshtml";
                public const string _PartialModalConfig = "_PartialModalConfig.cshtml";
                public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
                public const string _PartialTableTree = "_PartialTableTree.cshtml";
                public const string _PartialModalEdit = "_ModalEdit.cshtml";
            }
            public class Asset
            {
                public const string Image = "/sample/img/";
                public const string ImageThumb = "/sample/img/_thumb/";
            }

        }
    }
}

