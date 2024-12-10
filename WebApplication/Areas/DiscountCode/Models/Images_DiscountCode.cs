using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.DiscountCode.Models
{
    public class Images_DiscountCode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long DiscountCodeDetailPid { get; set; }
        public virtual DiscountCodeDetail DiscountCodeDetail { get; set; }
        public string Images { get; set; } 
        public long Order { get; set; }
        public virtual ICollection<MultiLang_Images_DiscountCode> MultiLang_Images_DiscountCodees { get; set; }

    }
}
