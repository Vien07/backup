
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;


namespace Admin.Course.Database
{
    public class Course
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Required")]
        public string UnsignedTitle { get; set; }

        public string Description { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public string Images { get; set; } = String.Empty;
        public string Slug { get; set; } = String.Empty;

        public decimal Price { get; set; }
        public decimal PriceDiscount { get; set; }

        public bool Hot { get; set; } = false;
        public bool New { get; set; } = false;

        [MaxLength(48, ErrorMessage = "Max 48")]
        public string SkillLevel { get; set; } = String.Empty;
        [MaxLength(4, ErrorMessage = "Max 4")]
        public string NumberOfLessons { get; set; } = String.Empty;
        [MaxLength(48, ErrorMessage = "Max 48")]
        public string Duration { get; set; } = String.Empty;

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


        public ICollection<CourseChapter> CourseChapters { get; set; }
        public ICollection<CourseCategory> CourseCategories { get; set; }
        public ICollection<Course_Member> Course_Members { get; set; }
    }

    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
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

