using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Admin.Models
{
    public class Visitor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public string Id { get; set; }
        public string IP { get; set; }
        public string DeviceName { get; set; }
        public string Brower { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LastOnlineTime { get; set; }
    }
}
