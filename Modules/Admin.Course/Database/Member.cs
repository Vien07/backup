
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace Admin.Course.Database
{
    public class Member
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public string PhoneNumber { get; set; }
        public string Password { get; set; } = String.Empty;
        public string Salt { get; set; } = String.Empty;


        public double Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }

        public ICollection<Course_Member> Course_Members { get; set; }
        public ICollection<Lecture_Member> Lecture_Members { get; set; }


    }

    public class MemberValidator : AbstractValidator<Member>
    {
        public MemberValidator()
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

