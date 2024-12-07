using Steam.Core.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Database
{
    public class OrderDetail : BaseEntity
    {
        public long OrderManagementPid { get; set; }
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal DiscountRate { get; set; }
    }
}
