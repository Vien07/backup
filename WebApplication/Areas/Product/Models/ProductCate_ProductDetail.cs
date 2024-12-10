using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Product.Models
{
    public class ProductCate_ProductDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ProductDetailPid { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public long ProductCatePid { get; set; }
        public decimal Price { get; set; }
        public virtual ProductCate ProductCate { get; set; }
        public bool Enable { get; set; } = true;
    }
}
