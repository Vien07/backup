using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Recruitment.Models
{
    public class MultiLang_RecruitmentDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public long RecruitmentDetailPid { get; set; }
        public virtual RecruitmentDetail RecruitmentDetail { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string TitleWithoutSign { get; set; } = string.Empty;
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Content { get; set; }
        public string Location { get; set; }
        public string Slug { get; set; }
        public string LangKey { get; set; }
        public string Salary { get; set; }
        public string Type { get; set; }
        public string Exp { get; set; }
        public string Rank { get; set; }
        public string Job { get; set; }
        public string WorkPlace { get; set; }
    }
}
