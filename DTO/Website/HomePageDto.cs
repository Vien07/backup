using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Website
{
    public class HomePageDto
    {
        public int Pid { get; set; }
        public string Images { get; set; } = string.Empty;
        //public string Link { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string TitleAlt { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string IntroLink { get; set; } = string.Empty;
    }
}
