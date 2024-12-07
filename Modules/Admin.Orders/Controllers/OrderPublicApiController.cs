using Admin.Orders.Models.Dto;
using Admin.Orders.Services;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Steam.Core.Infrastructure.HttpHandle;
using Steam.Core.Infrastructure.Web;

namespace Admin.Orders.Controllers
{
    public class OrderPublicApiController : ControllerBase
    {
        private readonly IOrderPublicApiService _orderPublicApiService;
        public OrderPublicApiController(IOrderPublicApiService orderPublicApiService)
        {
            _orderPublicApiService = orderPublicApiService;
        }

        [HttpPost]
        [Route("steam/[controller]/checkout")]
        public async Task<IActionResult> Checkout([FromForm] CheckoutModel model)
        {
            //set default value for OrderProductItems and payment method
            model.OrderProductItems = new List<OrderProductItemModel>()
            {
                new OrderProductItemModel() { ProductId = 1, Quantity = 2},
                new OrderProductItemModel() { ProductId = 2, Quantity = 2}
            };
            model.PaymentMethod = "PayOS";
            model.Description = "Thanh toán đơn hàng";

            var validator = new CheckoutModelValidator();
            ValidationResult validationResults = validator.Validate(model);

            if (!validationResults.IsValid)
            {
                var errorResponse = new ResponseErrorHelper()
                    .IsErrorBadRequest()
                    .WithFluentValidationDetails(validationResults.Errors);
                return BadRequest(errorResponse);
            }

            var response = await _orderPublicApiService.Checkout(model);
            return StatusCode(response.StatusCode, response.Response());
        }
    }
}
