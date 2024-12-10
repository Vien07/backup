using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Contact.Models
{
    public class MultiLang_Branch
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public int BranchPid { get; set; }
        public virtual Branch Branch { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        //public string Description { get; set; }
        public string Address { get; set; }
        public string LangKey { get; set; }
    }
}
