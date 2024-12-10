using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DTO.Recruitment
{
    public class CVDto
    {
        public string FullName { get; set; }
        public string Content { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public IFormFile File { get; set; }
        public int RecruitmentDetailPid { get; set; }
        public string Title { get; set; }
        public string NameRecruit { get; set; }
    }
}
