using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Configurations.Models
{
    public class EmailTemplate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long Order { get; set; }
        public string Group { get; set; }
        public bool Enabled { get; set; } = true;
        public bool IsPlainText { get; set; } 
        public string Title { get; set; } 
        public string Code { get; set; } 

        public virtual ICollection<MultiLang_EmailTemplate> MultiLang_EmailTemplates { get; set; }


    }
}
