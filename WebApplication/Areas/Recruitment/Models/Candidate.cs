using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Recruitment.Models
{
    public class Candidate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CV { get; set; }
        public long RecruitmentDetailPid { get; set; }
        public virtual RecruitmentDetail RecruitmentDetail { get; set; }

        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }
    }
}
