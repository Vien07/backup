using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Recruitment.Models
{
    public class Images_Recruitment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long RecruitmentDetailPid { get; set; }
        public virtual RecruitmentDetail RecruitmentDetail { get; set; }
        public string Images { get; set; } 
        public long Order { get; set; }
        public virtual ICollection<MultiLang_Images_Recruitment> MultiLang_Images_Recruitmentes { get; set; }

    }
}
