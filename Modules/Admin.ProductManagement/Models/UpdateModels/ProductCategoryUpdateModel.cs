using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.Models.UpdateModels
{
    public class ProductCategoryUpdateModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string Parameter { get; set; }
        public string WebsiteCatePage { get; set; }
        public string WebsiteDetailPage { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public int ParentID { get; set; }
    }
}
