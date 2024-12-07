
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Course.Models
{
    public class ParamSearch
    {
        public string Title { get; set; } = String.Empty;
        public string Note { get; set; } = String.Empty;
        public bool ? Enabled { get; set; }
        public int PageIndex { get; set; } = 1;
    }
}
