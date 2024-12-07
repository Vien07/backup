using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.MisaReponse
{
    public class MisaResponseProductDto
    {
        public decimal AvgUnitPrice { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid BranchId { get; set; }
        public int ItemType { get; set; }
        public Guid ItemCategoryId { get; set; }
        public string Barcode { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string Color { get; set; }
        public string ColourCode { get; set; }
        public string Size { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public bool IsItem { get; set; }
        public bool Inactive { get; set; }
        public Guid UnitId { get; set; }
        public string UnitName { get; set; }
        public string ItemCategoryName { get; set; }
        public string Picture { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<MisaResponseProductDetailDto> ListDetail { get; set; }
    }
}
