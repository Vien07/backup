
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.WebSetting.Models
{
    public class ParramSearch
    {

        public string Title { get; set; }
        public string Note { get; set; }
        public int PageIndex { get; set; } = 1;
    }
}
