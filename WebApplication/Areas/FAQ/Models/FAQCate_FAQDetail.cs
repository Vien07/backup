using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.FAQ.Models
{
    public class FAQCate_FAQDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long FAQDetailPid { get; set; }
        public virtual FAQDetail FAQDetail { get; set; }
        public long FAQCatePid { get; set; }
        public virtual FAQCate FAQCate { get; set; }
    }
}
