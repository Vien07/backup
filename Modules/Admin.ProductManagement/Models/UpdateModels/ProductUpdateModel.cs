using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.UpdateModels
{
    public class ProductUpdateModel
    {
        public string Title { get; set; }
        public string ProductPolicyIds { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public decimal SellingPrice { get; set; }
        public string Slug { get; set; }
        public string TypeProduct { get; set; } = string.Empty;
        public long CateID { get; set; }
        public string SubCate { get; set; }
        public string Images_Description { get; set; }
        public string Images_Alt { get; set; }
        public string Images_Caption { get; set; }
        public string BackImages_Description { get; set; }
        public string BackImages_Alt { get; set; }
        public string BackImages_Caption { get; set; }
    }
}
