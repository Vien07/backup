using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.OrderManagement
{
    public class OrderManagementDto
    {
        public long Pid { get; set; }
        public double? Order { get; set; }
        public bool Enabled { get; set; } = true;
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Provider { get; set; }
        public decimal Amount { get; set; }
        public string Data { get; set; }
    }
}
