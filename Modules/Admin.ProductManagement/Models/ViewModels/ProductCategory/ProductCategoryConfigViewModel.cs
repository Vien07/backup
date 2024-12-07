using Admin.ProductManagement.DataTransferObjects.ProductCategory;

namespace Admin.ProductManagement.Models.ViewModels.ProductCategory
{
    public class ProductCategoryConfigViewModel
    {
        public List<ProductCategoryConfigDto> Items { get; set; }
        public ProductCategoryConfigDto Item { get; set; }
    }
}
