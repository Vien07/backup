
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Database
{
    public class QuickToolBar : BaseEntity
    {


        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        public string? QuickToolBarBlock { get; set; } = String.Empty;

    }
    public class QuickToolBarValidator : AbstractValidator<QuickToolBar>
    {
        public QuickToolBarValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

