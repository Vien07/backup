using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Course.Models
{
    public class MemberModel
    {

    }
    public class MemberConfigDto
    {
        public int MinWidth { get; set; }
        public int MaxWidth { get; set; }
        public int PageSize { get; set; }
    }

    public class CourseMemberViewModel
    {
        public string CourseName { get; set; }
        public string Key { get; set; }
        public string StartDate { get; set; }
        public string ExpireDate { get; set; }
        public bool Actived { get; set; }
        public int Pid { get; set; }
    }
}
