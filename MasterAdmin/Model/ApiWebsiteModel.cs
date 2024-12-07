
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MasterAdmin.Models
{
    public class ApiWebsiteModel
    {
        public class SiteMapModel
        {
            public string loc { get; set; }
            public string lastmod { get; set; }
            public string image { get; set; }
            public string image_loc { get; set; }
            public string image_caption { get; set; }
            public string image_title { get; set; }
        }
    }
}
