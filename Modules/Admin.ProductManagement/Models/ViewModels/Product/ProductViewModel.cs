using Admin.ProductManagement.DataTransferObjects.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.ViewModels.Product
{
    public class ProductViewModel
    {
        public ProductDto Detail { get; set; }
        public List<ProductFileDto> Files { get; set; }
        public List<ProductDetailDto> ProductChildren { get; set; }
    }
}
