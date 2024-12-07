using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Orders.Models.Dto
{
    public class CheckoutModel
    {
        public CheckoutModel()
        {
            OrderProductItems = new List<OrderProductItemModel>();
        }

        public string? BuyerName { get; set; }

        public string? BuyerEmail { get; set; }

        public required string BuyerPhone { get; set; }

        public string? BuyerAddress { get; set; }

        public string? Description { get; set; }

        public required string PaymentMethod { get; set; }

        public string? CouponCode { get; set; }

        public IList<OrderProductItemModel> OrderProductItems { get; set; }

    }

    public class CheckoutModelValidator : AbstractValidator<CheckoutModel>
    {
        public CheckoutModelValidator()
        {
            RuleFor(x => x.BuyerName).NotEmpty().WithMessage("BuyerName can not be blank!");
            RuleFor(x => x.BuyerPhone).NotEmpty().WithMessage("BuyerPhone can not be blank!");
            RuleFor(x => x.BuyerAddress).NotEmpty().WithMessage("BuyerAddress can not be blank!");
        }
    }

}
