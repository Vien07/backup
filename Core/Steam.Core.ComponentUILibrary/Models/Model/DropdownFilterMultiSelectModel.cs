using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.Models
{
    public class DropdownFilterMultiSelectModel
    {
        public List<SelectControlData> Data { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string SelectedValue { get; set; }
        public string Size { get; set; } //small wide
        public bool FirstLoadLib { get; set; } = false; 

    }

}
