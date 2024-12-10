using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Recruitment.Models
{
    public class RecruitmentDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public string PicThumb { get; set; } = string.Empty;
        public string TagKey { get; set; } 
        public DateTime PublishDate { get; set; }
        public DateTime ? ExpireDate { get; set; }
        public long CounterView { get; set; } = 0;
        public int ? Amount { get; set; }
        public long Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }= DateTime.Now;
        public virtual ICollection<MultiLang_RecruitmentDetail> MultiLang_RecruitmentDetails { get; set; }
        public virtual ICollection<Images_Recruitment> Images_Recruitmentes { get; set; }
        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}
