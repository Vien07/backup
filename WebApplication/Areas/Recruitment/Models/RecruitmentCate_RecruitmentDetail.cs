using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Recruitment.Models
{
    public class RecruitmentCate_RecruitmentDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long RecruitmentDetailPid { get; set; }
        public virtual RecruitmentDetail RecruitmentDetail { get; set; }
        public long RecruitmentCatePid { get; set; }
        public virtual RecruitmentCate RecruitmentCate { get; set; }
    }
}
