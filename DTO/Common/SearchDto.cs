using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Common
{
    public class SearchDto
    {
        public int Page { get; set; }
        public string PaymentMethod { get; set; }
        public string IsPayment { get; set; }
        public int PageNumber { get; set; }
        public bool? Enable { get; set; }
        public string Key { get; set; }
        public string Cate { get; set; }
        public string OrderPid { get; set; }
    }
}
