using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.FAQ.Models
{
    public class MultiLang_Images_FAQ
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ImagesFAQPid { get; set; }
        public virtual Images_FAQ ImagesFAQ { get; set; }
        public string Caption { get; set; }
        public string LangKey { get; set; }
    } 
}
