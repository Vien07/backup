using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Banner.Models
{
    public class Banner_Page
    { 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public int BannerPid { get; set; } 
        public virtual Banner Banner { get; set; }
        public int PageId { get; set; } 
        public virtual Page Page { get; set; }

    }
}
