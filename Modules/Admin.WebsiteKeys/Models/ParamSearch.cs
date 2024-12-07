
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.WebsiteKeys.Models
{
    public class ParamSearch
    {

        public string KeySearch { get; set; }=String.Empty;
        public string Cate { get; set; } = "0";
        public string Type { get; set; } = "0";
        public string View { get; set; } = "customkey";
        public int PageIndex { get; set; } = 1;
    }
}
