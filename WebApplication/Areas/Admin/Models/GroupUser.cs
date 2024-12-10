using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Admin.Models
{
    public class GroupUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; } 

        [MaxLength(64, ErrorMessage = "Max 64")]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        public string Role { get; set; } = "Staff";
        public int Order { get; set; }
        public bool View { get; set; } = true;
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
