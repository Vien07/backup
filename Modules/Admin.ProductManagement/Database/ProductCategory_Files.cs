
using Steam.Core.Base.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.ProductManagement.Database
{ 
    public class ProductCategory_Files : BaseEntity
    {
        public long PostsCategoryId { get; set; }
        public string UploadFileName { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
    }
}
