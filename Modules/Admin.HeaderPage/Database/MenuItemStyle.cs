
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.HeaderPage.Database
{
    public class MenuItemStyle : BaseDatabaseModel
    {


        [Required(ErrorMessage = "Required")]
        public int Level { get; set; }
        public string? ItemBlock { get; set; } = String.Empty;
        public long MenuStylePid { get; set; } 

    }
    public class MenuItemStyleValidator : AbstractValidator<MenuItemStyle>
    {
        public MenuItemStyleValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

