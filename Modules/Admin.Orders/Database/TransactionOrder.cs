using Admin.Payments.Models;
using Steam.Core.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Orders.Database
{
    public class TransactionOrder : BaseEntity
    {
        public int OrderId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string? ResponseData { get; set; }
    }
}
