using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace CMS.Areas.Product.Models
{
    public class ProductComment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }

        public long CustomerPid { get; set; }
        public long ProductDetailPid { get; set; }
        public int Star { get; set; } = 0;
        public long? ParentId { get; set; }
        public long? ReplyId { get; set; } //customer reply comment of another customer
        public string Comment { get; set; } = string.Empty;
        public int Like { get; set; }
        public int Heart { get; set; }
        public int Share { get; set; }

        public bool Enabled { get; set; } = false;
        public bool Deleted { get; set; } = false;

        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;

    }
}
