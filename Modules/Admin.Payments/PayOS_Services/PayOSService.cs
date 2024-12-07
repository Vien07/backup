using Microsoft.Extensions.Configuration;
using Admin.Payments.Models;
using Net.payOS;
using Net.payOS.Types;

namespace Admin.Payments.PayOS_Services
{
    public class PayOSService : IPayOSService
    {
        private readonly IConfiguration _configuration;
        public PayOSService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<PaymentReturnDataModel> CreatePayOSPaymentLink(PaymentInformationModel model)
        {
            PayOS payOS = new PayOS(
               _configuration["PaymentConfigurations:PayOS:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment"),
               _configuration["PaymentConfigurations:PayOS:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment"),
               _configuration["PaymentConfigurations:PayOS:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment"),
               _configuration["PaymentConfigurations:PayOS:PAYOS_PARTNER_CODE"] ?? throw new Exception("Cannot find environment"));

            var items = new List<ItemData>();

            foreach (var item in model.PaymentProductItems)
            {
                items.Add(new ItemData(item.ProductName, item.Quantity, Convert.ToInt32(item.Price)));
            }

            long unixTimestampExpired = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds();

            var payment = await payOS.createPaymentLink(
                new PaymentData(
                    orderCode: model.OrderId,
                    amount: Convert.ToInt32(model.Amount),
                    description: model.Description,
                    items: items,
                    cancelUrl: _configuration["PaymentConfigurations:PayOS:CANCEL_URL"] ?? throw new Exception("Cannot find environment"),
                    returnUrl: _configuration["PaymentConfigurations:PayOS:RETURN_URL"] ?? throw new Exception("Cannot find environment"),
                    buyerName: model.BuyerName,
                    buyerEmail: model.BuyerEmail,
                    buyerPhone: model.BuyerPhone,
                    buyerAddress: model.BuyerAddress,
                    expiredAt: unixTimestampExpired));

            return new PaymentReturnDataModel
            {
                PaymentLink = payment.checkoutUrl,
                IsCheckoutWithPaymentLink = true
            };
        }
    }
}
