using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Admin.Models
{
    public class GroupAdminMenu
    {
        [Key]
        [MaxLength(24, ErrorMessage = "Max 24")]
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }

        [MaxLength(64, ErrorMessage = "Max 64")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public string Icon { get; set; }
        public int Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public string UpdateUser { get; set; }
        public System.Nullable<DateTime> UpdateDate { get; set; }
    }
}
