using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.Product
{
    public class ProductDetailDto
    {
        public long Pid { get; set; }
        public long ParentPid { get; set; }
        public string? Title { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }
        public string? ColorValue { get; set; }
        public string? Sku { get; set; }
        public string? Size { get; set; }
        public decimal SellingPrice { get; set; }
    }
}
