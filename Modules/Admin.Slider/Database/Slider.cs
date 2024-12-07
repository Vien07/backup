
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;

namespace Admin.Slider.Database
{
    public class Slider: BaseDatabaseModel
    {


        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string? Description { get; set; } = String.Empty;
        [Required(ErrorMessage = "Required")]
        public string Images { get; set; } = String.Empty;
        public string? Images_Alt { get; set; } = String.Empty;
        public string? Link { get; set; } = String.Empty;
        public string? Position { get; set; } = String.Empty;
        public string? TypeMedia { get; set; } = "image";
        public string? FilePath { get; set; } = String.Empty;


    }
    public class SliderValidator : AbstractValidator<Slider>
    {
        public SliderValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(nameof(Slider.Title) + " - " + ErrorCode.NotEmpty);
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }

        private bool CheckTypeImage(string postcode)
        {
            return true;
        }
    }


}

