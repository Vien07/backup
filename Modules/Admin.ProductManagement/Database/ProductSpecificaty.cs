
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Database
{
    public class ProductSpecificaty : BaseEntity
    {
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Value { get; set; } 
        [Required(ErrorMessage = "Required")]
        public string Group { get; set; }
    }
}

