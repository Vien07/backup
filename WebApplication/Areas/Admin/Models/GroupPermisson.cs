using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Admin.Models
{
    public class GroupPermisson
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Code { get; set; }

        public int GroupUserCode { get; set; }
        [ForeignKey("GroupUserCode")]
        public virtual GroupUser GroupUser { get; set; }

        [MaxLength(24, ErrorMessage = "Max 24")]
        [Required(ErrorMessage = "Required")]
        public string ModuleCode { get; set; }
        [ForeignKey("ModuleCode")]
        public virtual Module Module { get; set; }


        [MaxLength(24, ErrorMessage = "Max 24")]
        public string PermissonCode { get; set; }
        [ForeignKey("PermissonCode")]
        public virtual Permission Permission { get; set; }

        public int Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public string UpdateUser { get; set; }
        public System.Nullable<DateTime> UpdateDate { get; set; }
    }
}
