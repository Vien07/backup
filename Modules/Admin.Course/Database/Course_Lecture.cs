
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using FluentValidation;
using Admin.Course.Database;

namespace Admin.Course.Database
{
    public class Course_Lecture
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public int Order { get; set; }
        public bool Preview { get; set; } = false;
        public string Title { get; set; } = string.Empty;
        public int CourseChapterPid { get; set; }
        public CourseChapter CourseChapter { get; set; }

        public int LecturePid { get; set; }
        public Lecture Lecture { get; set; }
    }
}

