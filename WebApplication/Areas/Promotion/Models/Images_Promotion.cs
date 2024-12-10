using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Promotion.Models
{
    public class Images_Promotion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long PromotionDetailPid { get; set; }
        public virtual PromotionDetail PromotionDetail { get; set; }
        public string Images { get; set; } 
        public long Order { get; set; }
        public virtual ICollection<MultiLang_Images_Promotion> MultiLang_Images_Promotiones { get; set; }

    }
}
