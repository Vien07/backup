using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Promotion.Models
{
    public class PromotionCate_PromotionDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long PromotionDetailPid { get; set; }
        public virtual PromotionDetail PromotionDetail { get; set; }
        public long PromotionCatePid { get; set; }
        public virtual PromotionCate PromotionCate { get; set; }
    }
}
