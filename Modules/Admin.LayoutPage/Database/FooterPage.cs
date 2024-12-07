
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Database
{
    public class FooterPage : BaseEntity
    {


        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        public string? FooterBlock { get; set; } = String.Empty;
        public string? FooterPluginBlock { get; set; } = String.Empty;

    }
    public class FooterPageValidator : AbstractValidator<FooterPage>
    {
        public FooterPageValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

