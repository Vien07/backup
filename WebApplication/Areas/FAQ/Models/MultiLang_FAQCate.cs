using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.FAQ.Models
{
    public class MultiLang_FAQCate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public long FAQCatePid { get; set; }
        public virtual FAQCate FAQCate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string LangKey { get; set; }
    }
}
