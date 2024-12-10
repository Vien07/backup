using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Admin.Models
{
    public class Permission
    {
        [Key]
        [MaxLength(24, ErrorMessage = "Max 24")]
        [Required(ErrorMessage = "Required")]
        public string Code { get; set; }
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string Name { get; set; }
        public bool Locked { get; set; } = false;
        public ICollection<GroupPermisson> GroupPermissons { get; set; }
    }
}
