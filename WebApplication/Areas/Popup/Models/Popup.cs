using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Popup.Models
{
    public class Popup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Images { get; set; }
        public string Link { get; set; }
        public string Position { get; set; }
        public int DelayTime { get; set; } = 2;
        public string TargetLink { get; set; } = "_self";

        public int Order { get; set; }
        public int? Type { get; set; }
        public string DisplayType { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public System.Nullable<DateTime> UpdateDate { get; set; }
        public virtual ICollection<MultiLang_Popup> MultiLang_Popups { get; set; }
        public virtual ICollection<Popup_Page> Popup_Pages { get; set; }


    }
}
