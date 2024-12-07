using Admin.ProductManagement.DataTransferObjects.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.ViewModels.ProductCategory
{
    public class ListProductCategoryViewModel
    {
        public List<ProductCategoryDto> Data { get; set; }
        public int Level { get; set; } = 0;
    }
}
