using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.Authorization.Database
{
    public class Group_ModuleRole : BaseEntity
    {

        [Required(ErrorMessage = "Required")]
        public long Id_ModuleRole { get; set; }

        [Required(ErrorMessage = "Required")]
        public long Id_GroupRole { get; set; }



    }
    public class ModuleRoleGroupValidator : AbstractValidator<ModuleRole>
    {
        public ModuleRoleGroupValidator()
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

