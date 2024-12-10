using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Admin.Models
{
    public class Log
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string AdminUser { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int PidDetail { get; set; }
        public int PidCate { get; set; }
        public int Status { get; set; }
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string IP { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
