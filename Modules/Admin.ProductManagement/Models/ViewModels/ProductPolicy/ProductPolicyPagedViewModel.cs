using Admin.ProductManagement.DataTransferObjects.ProductPolicy;

namespace Admin.ProductManagement.Models.ViewModels.ProductPolicy
{
    public class ProductPolicyPagedViewModel
    {
        public List<ProductPolicyDto> Items { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
