using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.About
{
    public class AboutDto
    {
        public long Pid { get; set; }
        public string Title { get; set; }
        public string TitleAlt { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string TagKey { get; set; }
        public string OrgImages { get; set; }
        public bool Enabled { get; set; }
        public bool Default { get; set; }
        public string PublishDate { get; set; }
    }
}
