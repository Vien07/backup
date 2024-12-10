using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Admin.Models
{
    public class Module
    {
        [Key]
        [MaxLength(24, ErrorMessage = "Max 24")]
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        [MaxLength(64, ErrorMessage = "Max 64")]
        [Required(ErrorMessage = "Required")]
        public string  ModuleName { get; set; }
        public string Icon { get; set; }
        [MaxLength(128, ErrorMessage = "Max 128")]
        public string Url { get; set; }
        [MaxLength(128, ErrorMessage = "Max 128")]
        public string UrlRewrite { get; set; }
        public bool Locked { get; set; } = false;

        public int Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [MaxLength(24, ErrorMessage = "Max 24")]

        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]

        public string UpdateUser { get; set; }
        public System.Nullable<DateTime> UpdateDate { get; set; }
        public ICollection<GroupPermisson> GroupPermissons { get; set; }

    }
}
