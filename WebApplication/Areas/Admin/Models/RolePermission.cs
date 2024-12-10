using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Admin.Models
{
    public class RolePermission
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public string RoleCode { get; set; }
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string AdminMenuCode { get; set; }
        public long PermissionPid { get; set; }
        public bool Licensed { get; set; }
    }
}
