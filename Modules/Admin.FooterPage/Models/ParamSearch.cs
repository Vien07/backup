
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.FooterPage.Models
{
    public class ParamSearch
    {

        public string KeySearch { get; set; }=String.Empty;
        public string Cate { get; set; } = "0";
        public string Type { get; set; } = "0";
        public int PageIndex { get; set; } = 1;
    }
}
