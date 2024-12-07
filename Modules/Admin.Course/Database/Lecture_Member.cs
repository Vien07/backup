
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using FluentValidation;
using Admin.Course.Database;

namespace Admin.Course.Database
{
    public class Lecture_Member
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }

        public int LecturePid { get; set; }
        public Lecture Lecture { get; set; }

        public int MemberPid { get; set; }
        public Member Member { get; set; }

        public bool Status { get; set; } = false;

        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

