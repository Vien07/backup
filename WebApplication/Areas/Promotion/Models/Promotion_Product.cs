using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CMS.Areas.Product.Models;
using System;

namespace CMS.Areas.Promotion.Models
{
    public class Promotion_Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long PromotionPid { get; set; }
        public PromotionDetail PromotionDetail { get; set; }
        public long ProductPid { get; set; }
        public ProductDetail ProductDetail { get; set; }
        public long OptionPid { get; set; }
        public ProductOption ProductOption { get; set; }
        public decimal Price { get; set; }
    }
}
