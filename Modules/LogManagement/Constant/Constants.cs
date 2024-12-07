
using Microsoft.AspNetCore.Http;
using Steam.Core.Base.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.LogManagement.Constants
{

    public class Config
    {
        public class Route
        {
            public const string PageIndex = SystemInfo.VirtualFolder+ "/LogManagement";
            public const string PageEdit = SystemInfo.VirtualFolder+"/LogManagement/Edit";
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

            public const string _PartialUrl = "~/Views/LogManagement/_Partial/";
            public const string _PartialModel = "_PartialModal.cshtml";
            public const string _PartialTable = "_PartialTable.cshtml";
            public const string _PartialModalConfig = "_PartialModalConfig.cshtml";
            public const string _PartialPageTitle = "/Views/Shared/_PageTitle.cshtml";
        }
        public class Asset
        {
            public const string Image = "/LogManagement/img/";
            public const string ImageThumb = "/LogManagement/img/_thumb/";
        }

    }
}
