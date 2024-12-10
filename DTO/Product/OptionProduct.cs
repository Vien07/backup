using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product
{
    public class OptionProduct
    {
        public long Order { get; set; }
        public long ProductDetailPid { get; set; }
        public long ProductOptionPid { get; set; }
        public decimal Price { get; set; } = 0;
        public decimal PriceDiscount { get; set; } = 0;
    }
}
