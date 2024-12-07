
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.Collection.Database
{
    public class CollectionCate : BaseEntity
    {


        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string? Description { get; set; } = String.Empty;
        public string Images { get; set; } = String.Empty;
        public string Icon { get; set; } = String.Empty;
        public string TypeDisplayChild { get; set; } = String.Empty;


    }
    public class CollectionCateValidator : AbstractValidator<CollectionCate>
    {
        public CollectionCateValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }

        private bool CheckTypeImage(string postcode)
        {
            return true;
        }
    }


}

