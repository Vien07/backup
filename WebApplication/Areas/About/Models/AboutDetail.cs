using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Areas.About.Models
{
    public class AboutDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public int AboutCatePid { get; set; } = 1;
        public virtual AboutCate AboutCate { get; set; }
        [Required(ErrorMessage = "Required")]
        public string PicThumb { get; set; } = string.Empty;
        public string TagKey { get; set; } 
        public DateTime PublishDate { get; set; }
        public long CounterView { get; set; } = 0;
        public int Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool ShowTopMenu { get; set; } = true;
        public bool ShowFooter { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public bool Default { get; set; } = false;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public virtual ICollection<MultiLang_AboutDetail> MultiLang_AboutDetails { get; set; }

    }
}
