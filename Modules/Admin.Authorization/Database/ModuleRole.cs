using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.Authorization.Database
{
    public class ModuleRole:BaseEntity
    {

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        public Nullable<long>  IdParent { get; set; }
        public string? RolePath { get; set; }= String.Empty;
        public bool AllowAnonymous { get; set; } = false;
        public string? ApiKey { get; set; } = String.Empty;
        public bool Log { get; set; } = false;
        public string? ActionName { get; set; } = String.Empty;
    }
    public class ModuleRoleValidator : AbstractValidator<ModuleRole>
    {
        public ModuleRoleValidator()
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

