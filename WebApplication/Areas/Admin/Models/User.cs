using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Admin.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [MaxLength(24, ErrorMessage = "Max 24")]
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [MaxLength(128, ErrorMessage = "Max 64")]
        [Required(ErrorMessage = "Required")]
        public string FullName { get; set; }
        public string Email { get; set; }
        [MaxLength(11, ErrorMessage = "Max 11")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required")]
        public int GroupUserCode { get; set; }
        public virtual GroupUser GroupUser { get; set; }

        [MaxLength(128, ErrorMessage = "Max 128")]
        public string Avatar { get; set; }

        [MaxLength(24)]
        public string Salt { get; set; }
        [MaxLength(16)]
        public string IP { get; set; }
        public int Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public System.Nullable<DateTime> UpdateDate { get; set; }
        public System.Nullable<DateTime> RecoveryTime { get; set; }
        public string RecoveryString { get; set; }
    }
}
