using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Partner.Models
{
    public class MultiLang_Partner
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public int PartnerPid { get; set; }
        public virtual Partner Partner { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string LangKey { get; set; }
    }
}
