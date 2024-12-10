using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Convenience.Models
{
    public class MultiLang_Convenience
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public int ConveniencePid { get; set; }
        public virtual Convenience Convenience { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string LangKey { get; set; }
    }
}
