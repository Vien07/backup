using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Promotion.Models
{
    public class MultiLang_PromotionDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public long PromotionDetailPid { get; set; }
        public virtual PromotionDetail PromotionDetail { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string TitleWithoutSign { get; set; } = string.Empty;
        public string TitleSEO { get; set; } = string.Empty;
        public string DescriptionSEO { get; set; } = string.Empty;

        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Content { get; set; }
        public string Slug { get; set; }
        public string LangKey { get; set; }
        public string PromoProductListId { get; set; }
        public virtual PromotionCate PromotionCate { get; set; }

    }
}
