using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.ProductCategory
{
    public class ProductCategoryDto
    {
        public long Pid { get; set; }
        public double? Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;

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
