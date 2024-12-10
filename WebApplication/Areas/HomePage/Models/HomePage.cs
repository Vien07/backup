using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.HomePage.Models
{
    public class HomePage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Images { get; set; }
        public string BackgroundImage { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Type { get; set; }

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
        public virtual ICollection<MultiLang_HomePage> MultiLang_HomePages { get; set; }

    }
}
