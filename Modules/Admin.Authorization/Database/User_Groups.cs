
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.Authorization.Database
{
    public class User_Groups : BaseEntity
    {

        [Required(ErrorMessage = "Required")]
        public long Id_User { get; set; }

        [Required(ErrorMessage = "Required")]
        public long Id_GroupRole { get; set; }
    }
    public class UserGroupValidator : AbstractValidator<ModuleRole>
    {
        public UserGroupValidator()
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

