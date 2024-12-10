using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Order.Models
{
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ProductDetailPid { get; set; }
        public long ProductCatePid { get; set; }
        public long CustomerPid { get; set; }
        public int State { get; set; }
        public int PaymentMethod { get; set; }
        public bool VAT { get; set; } = false;
        public bool IsPayment { get; set; }
        public decimal ShipFee { get; set; } = 0;
        public decimal Deposit { get; set; } = 0;
        public string Note { get; set; }
        public string Code { get; set; }
        public decimal Total { get; set; }

        public string Address { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string CompanyName { get; set; }
        public string TaxCode { get; set; }
        public string CompanyAddress { get; set; }
        public long BackgroundPid { get; set; }

        public bool Deleted { get; set; } = false;
        public bool Enabled { get; set; } = true;

        public string DiscountCodeValue { get; set; }
        public string DiscountCodeType { get; set; }
        public string DiscountCode { get; set; }

        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public DateTime ExpiredDate { get; set; } = DateTime.Now;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
