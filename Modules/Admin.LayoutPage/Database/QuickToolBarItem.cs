
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.LayoutPage.Database
{
    public class QuickToolBarItem : BaseEntity
    {


        public string? ItemBlock { get; set; } = String.Empty;
        public string Key { get; set; } = String.Empty;
        public long QuickToolBarPid { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string? Value { get; set; } = String.Empty;
        public string? Icon { get; set; } = String.Empty;
    }
    public class QuickToolBarItemValidator : AbstractValidator<QuickToolBarItem>
    {
        public QuickToolBarItemValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

