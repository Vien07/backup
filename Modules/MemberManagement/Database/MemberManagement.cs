
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.MemberManagement.Database
{
    public class MemberManagement: BaseEntity
    {


       
        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Required")]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; } = null!;
        public string? BirthDay { get; set; } = String.Empty;
        public string? TimeOfBirth { get; set; } = String.Empty;

        public string? Phone { get; set; }
        public string? NickName { get; set; } 
        public int? Sex { get; set; } 

        public string? Password { get; set; }
        public string? Token { get; set; } = null!;
        public string? Description { get; set; } 
        public string? Images { get; set; }
        public DateTime? LastTimeActive { get; set; } = DateTime.Now;
        public string? ResetToken { get; set; }
        public bool? IsVerify { get; set; } = false;
    }
    public class MemberManagementValidator : AbstractValidator<MemberManagement>
    {
        public MemberManagementValidator()
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

