using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects.Product;

namespace Admin.ProductManagement.Models.ViewModels.Product
{
    public class ProductPagedViewModel
    {
        public List<ProductDto> Items { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
