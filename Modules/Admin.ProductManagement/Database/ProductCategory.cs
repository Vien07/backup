
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Database
{
    public class ProductCategory : BaseEntity
    {
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string? Icon { get; set; }
        public string? Description { get; set; }
        public string? URL { get; set; }
        public string? Event { get; set; }
        public int ParentID { get; set; }
        public int ShowLevel { get; set; }
        public string? Images { get; set; }
        public string? Banner { get; set; }
        public string? Banner_Alt { get; set; }
        public string? BannerFilePath { get; set; }
        public string? FilePath { get; set; }
        public string? Images_Alt { get; set; }
        public string? ModuleCode { get; set; }
        public string Path { get; set; }
        public int RootParentID { get; set; }
        public string? WebsiteCatePage { get; set; } = string.Empty;
        public string? WebsiteDetailPage { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? Parameter { get; set; }
    }
}

