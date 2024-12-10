using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Cart
{
    public static class EnumOrder
    {
        public enum OrderState
        {
            Waiting = 0,
            Success = 1,
            Canceled = 2
        }

        public enum PaymentMethod
        {
            EWallet = 0,
            iBanking = 1,
            //Momo = 2,
        }
        public enum PaymentStatus
        {
            Unpaid = 0,
            Paid = 1,
        }
    }
}
