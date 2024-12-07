
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.ProductManagement.Database
{
    public class ProductDetail_PostDetail : BaseEntity
    {
        [Required(ErrorMessage = "Required")]
        public long ProductID { get; set; }
        [Required(ErrorMessage = "Required")]
        public long PostID { get; set; }
    }
}

