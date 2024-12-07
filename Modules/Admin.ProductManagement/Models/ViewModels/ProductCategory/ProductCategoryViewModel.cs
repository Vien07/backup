using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects;
using Admin.ProductManagement.DataTransferObjects.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.ViewModels.ProductCategory
{
    public class ProductCategoryViewModel
    {
        public ProductCategoryDto Detail { get; set; } = new();
        public List<ProductCategoryFilesDto> ListFiles { get; set; } = new();
    }
}
