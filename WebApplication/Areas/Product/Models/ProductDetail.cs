using CMS.Areas.Customer.Models;
using CMS.Areas.Promotion.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Product.Models
{
    public class ProductDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public string Code { get; set; }
        public string PicThumb { get; set; }
        public string TagKey { get; set; } = string.Empty;
        public string SlugTagKey { get; set; }
        public string Size { get; set; }
        public string Tiki { get; set; }
        public string Shopee { get; set; }
        public string Lazada { get; set; }
        public long CounterView { get; set; } = 0;
        public long Order { get; set; }
        public bool IsHot { get; set; } = false;
        public bool IsNew { get; set; } = false;
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public decimal Price { get; set; } = 0;
        public decimal PriceDiscount { get; set; } = 0;
        public long Stock { get; set; } = 0;
        public int UserAmount { get; set; }
        public int Level { get; set; }
        public int Cycle { get; set; }

        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public virtual ICollection<MultiLang_ProductDetail> MultiLang_ProductDetails { get; set; }
        public virtual ICollection<Images_Product> Images_Productes { get; set; }
        public virtual ICollection<ProductCate_ProductDetail> ProductCate_ProductDetails { get; set; }
        public virtual ICollection<ProductOption_ProductDetail> ProductOption_ProductDetails { get; set; }
        public virtual ICollection<ProductColor_ProductDetail> ProductColor_ProductDetails { get; set; }
        public ICollection<Promotion_Product> Promotion_Products { get; set; }

    }
}
