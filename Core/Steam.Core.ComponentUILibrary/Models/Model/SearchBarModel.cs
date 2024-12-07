using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentUILibrary.Models
{
    public class SearchBarModel
    {
        public bool ShowActiveSelect { get; set; } = true;
        public bool ShowSearchBox { get; set; } = true;
        public string ActiveSelectClass { get; set; } = string.Empty;
        public string SearchBoxClass { get; set; } = string.Empty;
        public List<CustomSelect>? CustomSelects { get; set; }
        public string Action { get; set; } = string.Empty;
        public bool FirstLoadLib { get; set; } = false;
    }
    public class CustomSelect
    {
        public List<Dictionary<string,string>> Data { get; set; } = new List<Dictionary<string,string>>();
        public string Class { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
    }
}
