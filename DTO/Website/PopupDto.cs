using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Website
{
    public class PopupDto
    {
        public string Link { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string TitleAlt { get; set; }
        public int? Type { get; set; }
        public string TypeName { get; set; } = "";
        public string DisplayType { get; set; }
        public string EmbedCode { get; set; }
        public int DelayTime { get; set; }
        public string TargetLink { get; set; } = "_self";
    }
}
