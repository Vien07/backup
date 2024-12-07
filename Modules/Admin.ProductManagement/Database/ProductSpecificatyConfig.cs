
using Steam.Core.Base.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.ProductManagement.Database
{
    public class ProductSpecificatyConfig : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
