using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Payments.Models
{
    public enum PaymentMethod
    {
        Cash = 1,

        BankTransfer = 2,

        PayOS = 3,
    }

    public static class PaymentMethodExtensions
    {
        public static PaymentMethod GetPaymentType(string paymentType)
        {
            return (PaymentMethod)Enum.Parse(typeof(PaymentMethod), paymentType);
        }
    }

}
