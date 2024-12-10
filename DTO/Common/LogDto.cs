using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Common
{
    public class LogDto
    {
        public string Description { get; set; }
        public string AdminUser { get; set; }
        public DateTime LoginTime { get; set; }
        public string IP { get; set; }
        public string Url { get; set; } = string.Empty;
        public int PidCate { get; set; }
        public int PidDetail { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public int PidUser { get; set; }
    }
}
