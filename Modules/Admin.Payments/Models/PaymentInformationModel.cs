using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Payments.Models
{
    public class PaymentInformationModel
    {
        public PaymentInformationModel()
        {
            PaymentProductItems = new List<PaymentProductItemModel>();
        }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string? Description { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string? BuyerName { get; set; }

        public string? BuyerEmail { get; set; }

        public required string BuyerPhone { get; set; }

        public string? BuyerAddress { get; set; }

        public IList<PaymentProductItemModel> PaymentProductItems { get; set; }
    }
}
