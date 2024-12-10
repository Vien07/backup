using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Configurations.Models
{
    public class MultiLang_EmailTemplate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public long EmailTemplatePid { get; set; }
        public virtual EmailTemplate EmailTemplate { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string LangKey { get; set; }

    }
}
