using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComponentUILibrary.Models
{
    public class NiceSelectModel
    {
        public List<SelectControlData> Data { get; set; }
        public string Id { get; set; }
        public int Search { get; set; } = 0;
        public string Name { get; set; }
        public string SelectedValue { get; set; }
        public string DefaultValue { get; set; } = "0";
        public string DefaultText { get; set; } = "--Chọn---";
        public bool IsRequire { get; set; } = true;
        public string Class { get; set; }
        public string Event { get; set; }
        public string Size { get; set; } //small wide
        public bool FirstLoadLib { get; set; } //small wide
    }
}
