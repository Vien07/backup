using Admin.ProductManagement.DataTransferObjects.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.ViewModels.Product
{
    public class ProductConfigViewModel
    {
        public List<ProductConfigDto> Items { get; set; }
        public ProductConfigDto Item { get; set; }
    }
}
