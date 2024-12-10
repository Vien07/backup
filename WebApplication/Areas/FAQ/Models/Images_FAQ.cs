using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.FAQ.Models
{
    public class Images_FAQ
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long FAQDetailPid { get; set; }
        public virtual FAQDetail FAQDetail { get; set; }
        public string Images { get; set; } 
        public long Order { get; set; }
        public virtual ICollection<MultiLang_Images_FAQ> MultiLang_Images_FAQes { get; set; }

    }
}
