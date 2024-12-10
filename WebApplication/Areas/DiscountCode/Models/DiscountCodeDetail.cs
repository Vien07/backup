using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.DiscountCode.Models
{
    public class DiscountCodeDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public string PicThumb { get; set; } = string.Empty;
        public string TagKey { get; set; }
        public string SlugTagKey { get; set; }
        public DateTime PublishDate { get; set; }
        public long CounterView { get; set; } = 0;
        public long Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool IsHot { get; set; } = false;
        public bool Deleted { get; set; } = false;

        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        [MaxLength(10, ErrorMessage = "Max 10")]
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxQuantity { get; set; }
        public int UsedQuantity { get; set; } = 0;
        public decimal MinTotal { get; set; }
        public decimal DiscountCodeValue { get; set; }
        public string DiscountCodeType { get; set; }


        public virtual ICollection<MultiLang_DiscountCodeDetail> MultiLang_DiscountCodeDetails { get; set; }
        public virtual ICollection<Images_DiscountCode> Images_DiscountCodees { get; set; }
        public virtual ICollection<DiscountCodeCate_DiscountCodeDetail> DiscountCodeCate_DiscountCodeDetails { get; set; }
    }
}
