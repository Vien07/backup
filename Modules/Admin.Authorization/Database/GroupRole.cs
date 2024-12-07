using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.Authorization.Database
{
    public class GroupRole : BaseEntity
    {

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }


    }
    public class GroupRoleValidator : AbstractValidator<GroupRole>
    {
        public GroupRoleValidator()
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

