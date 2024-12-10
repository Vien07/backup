using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Cart
{
    public class OrderDto
    {
        public long Pid { get; set; }
        public long CustomerId { get; set; }
        public decimal Total { get; set; }
        public string TotalString { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FullAddress { get; set; }
    }
}
