
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.HomePage.Database
{
    public class HomePage: BaseDatabaseModel
    {


        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Required")]
        public string? SourceData { get; set; } = String.Empty;
        public string? ListItemHtml { get; set; } = String.Empty;
        public string? Section { get; set; } = String.Empty;


    }
    public class HomePageValidator : AbstractValidator<HomePage>
    {
        public HomePageValidator()
        {
            //RuleFor(x => x.Title).NotEmpty().WithMessage("Chưa nhập tiêu đề!"); ;
            //RuleFor(x => x.Images).NotEmpty().WithMessage("Chưa nhập hình ảnh!");
            //RuleFor(x => x.Images).Must(CheckTypeImage).WithMessage("Hình ảnh không đúng định dạng!");
        }

        private bool CheckTypeImage(string postcode)
        {
            return true;
        }
    }


}

