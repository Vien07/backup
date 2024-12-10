using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Shared.Models
{
    public class ModulePreview
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public string Obj { get; set; } = string.Empty;
        public string PicThumb { get; set; } = string.Empty;
        public string ObjDetail { get; set; } = string.Empty;
        [MaxLength(2, ErrorMessage = "Max 2")]
        public string ModuleId { get; set; } = string.Empty;
        [MaxLength(2, ErrorMessage = "Max 2")]
        public string LangKey { get; set; }
        public bool IsEditPicThumb { get; set; } = false;
    }
}
