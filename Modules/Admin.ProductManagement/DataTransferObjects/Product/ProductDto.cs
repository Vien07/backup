using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.Product
{
    public class ProductDto
    {
        public long Pid { get; set; }
        public double? Order { get; set; }
        public bool Enabled { get; set; }
        public string? Title { get; set; }
        public string? Sku { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public decimal SellingPrice { get; set; }
        public string? Images { get; set; }
        public string? BackImages { get; set; }
        public string? FilePath { get; set; }
        public string? BackFilePath { get; set; }
        public string? Images_Caption { get; set; }
        public string? BackImages_Caption { get; set; }
        public string? Images_Description { get; set; }
        public string? BackImages_Description { get; set; }
        public string? Images_Alt { get; set; }
        public string? BackImages_Alt { get; set; }
        public string? ProductPolicyIds { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? Link { get; set; }
        public string? Position { get; set; }
        public long CateID { get; set; }
        public string? SubCate { get; set; }
        public bool Deleted { get; set; } = false;
        public string? CreateUser { get; set; }
        public string? UpdateUser { get; set; }
        public string? TypeProduct { get; set; }
        public List<long>? PostIds { get; set; } = new List<long>();
    }
}
