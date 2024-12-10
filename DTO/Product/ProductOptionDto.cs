using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product
{
    public class ProductOptionDto
    {
        public int OptionId { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }
        public decimal PriceDiscount { get; set; }
        public string PriceString { get; set; }
        public string PriceDiscountString { get; set; }
    }
}
