
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;


namespace Admin.Course.Database
{
    public class CourseCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public int CoursePid { get; set; }
        public Course Course { get; set; }
        public int CategoryPid { get; set; }
    }
}

