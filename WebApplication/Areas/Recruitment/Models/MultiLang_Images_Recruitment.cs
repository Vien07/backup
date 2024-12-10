using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Recruitment.Models
{
    public class MultiLang_Images_Recruitment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ImagesRecruitmentPid { get; set; }
        public virtual Images_Recruitment ImagesRecruitment { get; set; }
        public string Caption { get; set; }
        public string LangKey { get; set; }
    } 
}
