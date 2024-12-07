
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.HeaderPage.Database
{
    public class MenuStyle : BaseDatabaseModel
    {


        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string? MenuBlock { get; set; } = String.Empty;

    }
    public class MenuStyleValidator : AbstractValidator<MenuStyle>
    {
        public MenuStyleValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

