using Admin.Orders.Models;
using Admin.Orders.Models.Dto;
using Admin.Payments.Models;
using Admin.Payments.Services;
using Steam.Core.Infrastructure.HttpHandle;
using Steam.Core.Infrastructure.Web;
using Steam.Core.Utilities.STeamHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Orders.Services
{
    public class OrderPublicApiService : IOrderPublicApiService
    {
        private readonly IPaymentService _paymentService;
        private readonly ILoggerHelper _logger;

        public OrderPublicApiService(IPaymentService paymentService, ILoggerHelper logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        public async Task<IApplicationResponse> Checkout(CheckoutModel model)
        {
            try
            {
                var payment = new PaymentInformationModel
                {
                    PaymentMethod = PaymentMethodExtensions.GetPaymentType(model.PaymentMethod),
                    OrderId = GenerateOrderCode(),
                    BuyerName = model.BuyerName,
                    BuyerAddress = model.BuyerAddress,
                    BuyerPhone = model.BuyerPhone,
                    BuyerEmail = model.BuyerEmail,
                    Description = model.Description
                };

                foreach (var product in model.OrderProductItems)
                {
                    var paymentProductItem = new PaymentProductItemModel
                    {
                        ProductName = $"Sản phẩm sun phong thủy {product.ProductId}",
                        Quantity = product.Quantity,
                        Price = 1000
                    };

                    payment.PaymentProductItems.Add(paymentProductItem);
                    payment.Amount += paymentProductItem.Price * paymentProductItem.Quantity;
                }

                var orderReturn = new OrderReturnDataVm()
                {
                    OrderId = payment.OrderId,
                };


                if (payment.PaymentMethod is not PaymentMethod.Cash and not PaymentMethod.BankTransfer)
                {
                    orderReturn.Data = await _paymentService.ProcessPayment(payment);
                }

                return new ResponseSuccessfulDataHelper<OrderReturnDataVm>(orderReturn)
                    .WithFriendlyMessage("Checkout ordered successfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ResponseErrorHelper()
                    .IsErrorInternalServerError()
                    .WithDebugMessage(ex.Message)
                    .WithFriendlyMessage("Checkout order failed");
            }
        }

        private int GenerateOrderCode()
        {
            Random rand = new Random();
            int orderCode = (int)((DateTimeOffset.Now.Ticks % 1000000) + rand.Next(1000, 9999));
            return orderCode;
        }
    }
}
