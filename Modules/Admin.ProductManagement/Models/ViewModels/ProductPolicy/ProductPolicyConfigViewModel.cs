using Admin.ProductManagement.Database;
using Admin.ProductManagement.DataTransferObjects.ProductPolicy;

namespace Admin.ProductManagement.Models.ViewModels.ProductPolicy
{
    public class ProductPolicyConfigViewModel
    {
        public List<ProductPolicyConfigDto> Items { get; set; }
        public ProductPolicyConfigDto Item { get; set; }
    }
}
