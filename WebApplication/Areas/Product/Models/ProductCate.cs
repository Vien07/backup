using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Product.Models
{
    public class ProductCate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public int ParentId { get; set; }
        public bool IsShowHome { get; set; } = false;
        public string ParentRoute { get; set; }
        public string Code { get; set; }
        public string PicThumb { get; set; }
        public bool isLocked { get; set; } = false;
        public long Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public System.Nullable<DateTime> UpdateDate { get; set; }
        public int Months { get; set; } = 0;
        public virtual ICollection<ProductCate_ProductDetail> ProductCate_ProductDetails { get; set; }
        public virtual ICollection<MultiLang_ProductCate> MultiLang_ProductCates { get; set; }
    }
}
