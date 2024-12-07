
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Database
{
    public class SliderItem : BaseEntity
    {


        public string? ItemBlock { get; set; } = String.Empty;
        public string Key { get; set; } = String.Empty;
        public long SliderPid { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string? Description { get; set; } = String.Empty;
        [Required(ErrorMessage = "Required")]
        public string Images { get; set; } = String.Empty;
        public string? Images_Alt { get; set; } = String.Empty;
        public string? Link { get; set; } = String.Empty;
        public string? VideoLink { get; set; } = String.Empty;
        public string? Position { get; set; } = String.Empty;
        public string? TypeMedia { get; set; } = "image";
        public string? FilePath { get; set; } = String.Empty;
    }
    public class SliderItemValidator : AbstractValidator<SliderItem>
    {
        public SliderItemValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

