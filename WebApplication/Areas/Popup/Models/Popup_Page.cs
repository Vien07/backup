using CMS.Areas.Banner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Popup.Models
{
    public class Popup_Page
    { 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public int PopupPid { get; set; } 
        public virtual Popup Popup { get; set; }
        public int PageId { get; set; } 
        public virtual Page Page { get; set; }

    }
}
