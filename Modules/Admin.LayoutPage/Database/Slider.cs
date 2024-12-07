
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Database
{
    public class Slider : BaseEntity
    {


        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        public string? SliderBlock { get; set; } = String.Empty;

    }
    public class SliderValidator : AbstractValidator<Slider>
    {
        public SliderValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

