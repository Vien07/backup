using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.DiscountCode.Models
{
    public class DiscountCodeCate_DiscountCodeDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long DiscountCodeDetailPid { get; set; }
        public virtual DiscountCodeDetail DiscountCodeDetail { get; set; }
        public long DiscountCodeCatePid { get; set; }
        public virtual DiscountCodeCate DiscountCodeCate { get; set; }
    }
}
