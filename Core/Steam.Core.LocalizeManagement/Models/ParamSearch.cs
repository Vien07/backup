
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Steam.Core.LocalizeManagement.Models
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
