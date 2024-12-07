
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Database
{
    public class HeaderPage: BaseEntity
    {


        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string? HeaderBlock { get; set; } = String.Empty;

    }
    public class HeaderPageValidator : AbstractValidator<HeaderPage>
    {
        public HeaderPageValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

