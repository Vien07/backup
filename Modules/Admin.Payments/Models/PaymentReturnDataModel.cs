using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Payments.Models
{
    public class PaymentReturnDataModel
    {
        public string? PaymentLink { get; set; }

        public bool IsCheckoutWithPaymentLink { get; set; }
    }
}
