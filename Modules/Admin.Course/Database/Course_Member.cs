
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using FluentValidation;
using Admin.Course.Database;

namespace Admin.Course.Database
{
    public class Course_Member
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }

        public string Key { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public int CoursePid { get; set; }
        public Course Course { get; set; }

        public int MemberPid { get; set; }
        public Member Member { get; set; }

        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

