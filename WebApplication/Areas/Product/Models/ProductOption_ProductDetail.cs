using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Product.Models
{
    public class ProductOption_ProductDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ProductDetailPid { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public long ProductOptionPid { get; set; }
        public virtual ProductOption ProductOption { get; set; }

        public decimal Price { get; set; } = 0;
        public decimal PriceDiscount { get; set; } = 0;
        public bool IsSoldOut { get; set; } = false;
        public bool Status { get; set; } = true;
    }
}
