
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.FooterPage.Database
{
    public class FooterItem : BaseDatabaseModel
    {


        public string? ItemBlock { get; set; } = String.Empty;
        public string Key { get; set; } = String.Empty;
        public long FooterPagePid { get; set; } 

    }
    public class FooterItemValidator : AbstractValidator<FooterItem>
    {
        public FooterItemValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            
        }

      
    }


}

