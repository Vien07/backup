using Admin.ProductManagement.DataTransferObjects.ProductSpecificaty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.ViewModels.ProductSpecificaty
{
    public class ProductSpecificatyPagedViewModel
    {
        public List<ProductSpecificatyDto> Items { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
