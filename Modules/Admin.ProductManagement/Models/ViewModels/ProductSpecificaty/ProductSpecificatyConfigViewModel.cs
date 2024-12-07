using Admin.ProductManagement.DataTransferObjects.ProductSpecificaty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.ViewModels.ProductSpecificaty
{
    public class ProductSpecificatyConfigViewModel
    {
        public List<ProductSpecificatyConfigDto> Items { get; set; }
        public ProductSpecificatyConfigDto Item { get; set; }
    }
}
