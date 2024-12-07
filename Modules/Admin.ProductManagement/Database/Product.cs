
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Database
{
    public class Product : BaseEntity
    {
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string PublishDate { get; set; }
        public Guid MisaProductID { get; set; }
        public Guid UnitID { get; set; }
        public string? Sku { get; set; } = String.Empty;
        public string? Slug { get; set; } = String.Empty;
        public string? ProductDimension { get; set; } = String.Empty;
        public string? StorageInstruction { get; set; } = String.Empty;
        public string? DeliveryPolicy { get; set; } = String.Empty;
        public string? ExchangePolicy { get; set; } = String.Empty;
        public decimal SellingPrice { get; set; }
        public string? Description { get; set; } = String.Empty;
        public string? Content { get; set; } = String.Empty;
        public string? Images { get; set; } = String.Empty;
        public string? BackImages { get; set; } = String.Empty;
        public string? FilePath { get; set; } = String.Empty;
        public string? BackFilePath { get; set; } = String.Empty;
        public string? Images_Caption { get; set; } = String.Empty;
        public string? BackImages_Caption { get; set; } = String.Empty;
        public string? Images_Description { get; set; } = String.Empty;
        public string? BackImages_Description { get; set; } = String.Empty;
        public string? Images_Alt { get; set; } = String.Empty;
        public string? BackImages_Alt { get; set; } = String.Empty;
        public string? Link { get; set; } = String.Empty;
        public string? Position { get; set; } = String.Empty;
        public long CateID { get; set; }
        public string? SubCate { get; set; }
        public string? TypeProduct { get; set; }
        public string? ProductPolicyIds { get; set; }
        public bool isNew { get; set; } = false;
    }
}

