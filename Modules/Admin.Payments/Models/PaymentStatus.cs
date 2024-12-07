using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Payments.Models
{
    public enum PaymentStatus
    {
        Pending = 1,

        Paid = 2,

        Failed = 3,

        Refunded = 4,

        Cancelled = 5
    }
}
