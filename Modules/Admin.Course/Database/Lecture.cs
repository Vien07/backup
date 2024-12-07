
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;


namespace Admin.Course.Database
{
    public class Lecture
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Required")]
        public string UnsignedTitle { get; set; }

        public string Description { get; set; } = String.Empty;
        public string Images { get; set; } = String.Empty;
        public string Slug { get; set; } = String.Empty;

        [MaxLength(12, ErrorMessage = "Max 12")]
        public string Type { get; set; }

        public string Video { get; set; } = String.Empty;
        [MaxLength(12, ErrorMessage = "Max 12")]
        public string TimeOfVideo { get; set; } = string.Empty;


        [MaxLength(12, ErrorMessage = "Max 12")]
        public string NumberOfQuestion { get; set; } = string.Empty;
        public string Content { get; set; } = String.Empty; //question text

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
        public ICollection<Course_Lecture> Course_Lectures { get; set; }
        public ICollection<Lecture_Member> Lecture_Members { get; set; }

    }

    public class LectureValidator : AbstractValidator<Lecture>
    {
        public LectureValidator()
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

