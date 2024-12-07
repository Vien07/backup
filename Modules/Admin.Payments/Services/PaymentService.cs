using Admin.Payments.Models;
using Admin.Payments.PayOS_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Payments.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPayOSService _payOSService;
        public PaymentService(IPayOSService payOSService)
        {
            _payOSService = payOSService;
        }

        public IList<string> GetActivePaymentMethods()
        {
            //var paymentOptionConfigs = _configuration.GetValue<string>("Global.PaymentTypes").Split(",");
            //var paymentTypes = new List<string>();
            //foreach (var paymentType in paymentOptionConfigs)
            //{
            //    if (PaymentTypeExists(paymentType))
            //    {
            //        paymentTypes.Add(paymentType);
            //    }
            //}
            //return paymentTypes;
            return new List<string> { "Cash", "BankTransfer", "PayOS" };
        }

        private bool PaymentTypeExists(string paymentType)
        {
            return Enum.GetNames(typeof(PaymentMethod)).Contains(paymentType);
        }

        public async Task<PaymentReturnDataModel> ProcessPayment(PaymentInformationModel paymentInfo)
        {
            switch (paymentInfo.PaymentMethod)
            {
                case PaymentMethod.PayOS:
                    return await _payOSService.CreatePayOSPaymentLink(paymentInfo);

                default:
                    return new PaymentReturnDataModel() { IsCheckoutWithPaymentLink = false };
            }
        }

    }
}
