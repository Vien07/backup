using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Recruitment
{
    public class RecruitmentDto
    {
        public string Title { get; set; }
        public string TitleAlt { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public string Slug { get; set; }
        public int? Amount { get; set; }
        public long Pid { get; set; }
        public long Order { get; set; }
        public string PicThumb { get; set; }
        public string PublicDate { get; set; }
        public string TagKey { get; set; }
        public string Location { get; set; }
        public string? ExpiredDate { get; set; }
        public string PicFull { get; set; }
        public string Salary { get; set; }
        public string Type { get; set; }
        public string Exp { get; set; }

    }
}
