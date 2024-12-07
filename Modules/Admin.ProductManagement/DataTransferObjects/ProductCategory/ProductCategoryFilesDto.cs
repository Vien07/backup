using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ProductManagement.DataTransferObjects.ProductCategory
{
    public class ProductCategoryFilesDto
    {
        public long PostsCategoryId { get; set; }
        public string UploadFileName { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
    }
}
