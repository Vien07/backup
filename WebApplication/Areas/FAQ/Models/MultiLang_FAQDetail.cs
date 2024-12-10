using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.FAQ.Models
{
    public class MultiLang_FAQDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public long FAQDetailPid { get; set; }
        public virtual FAQDetail FAQDetail { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string TitleWithoutSign { get; set; } = string.Empty;
        public string TitleSEO { get; set; } = string.Empty;
        public string DescriptionSEO { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Answer { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Content { get; set; }
        public string Slug { get; set; }
        public string LangKey { get; set; }
        public virtual FAQCate FAQCate { get; set; }

    }
}
