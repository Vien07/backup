
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Database
{
    public class ProductDetail : BaseEntity
    {
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public int ParentPid { get; set; }
        public Guid MisaProductID { get; set; }
        public string? Sku { get; set; }
        public string? Barcode { get; set; }
        public decimal SellingPrice { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? ColorCode { get; set; }
    }
}

