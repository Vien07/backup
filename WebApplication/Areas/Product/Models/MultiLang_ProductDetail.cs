using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Product.Models
{
    public class MultiLang_ProductDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public long ProductDetailPid { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string TitleSEO { get; set; }
        public string TitleWithoutSign { get; set; } = string.Empty;
        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Unit { get; set; }
        public string DescriptionSEO { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Content { get; set; }
        public string Content2 { get; set; }
        public string Material { get; set; }
        public string ShortContent { get; set; }
        public string Slug { get; set; }
        public string LangKey { get; set; }

    }
}
