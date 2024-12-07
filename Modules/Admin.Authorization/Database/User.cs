
using FluentValidation;
using Steam.Core.Base.Models;

namespace Admin.Authorization.Database
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Role { get; set; } = "Everyone";
        public bool IsActive { get; set; } = false;
        public string? Token { get; set; } = "";
        public string Password { get; set; } = "";
        public string LastIPLogin { get; set; } = "";
        public DateTime? LastLogin { get; set; }
        public string? TokenUpdate { get; set; } 
        public DateTime? LasTimeChangePassword { get; set; }
        public string? Image { get; set; } = string.Empty;

        //public User(string userName, string name, string password, string role)
        //{
        //    UserName = userName;
        //    Name = name;
        //    Password = password;
        //    Role = role;
        //}
    }

    public class LoginUser
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class RegisterUser
    {
        public string Name { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "Everyone";
    }
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
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

