
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using FluentValidation;


namespace Admin.Course.Database
{
    public class CourseChapter
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public int Order { get; set; }
        public int CoursePid { get; set; }
        public Course Course { get; set; }

        public ICollection<Course_Lecture> Course_Lectures { get; set; }

    }
}

