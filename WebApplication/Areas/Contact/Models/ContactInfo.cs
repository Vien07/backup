using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Contact.Models
{
    public class ContactInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public bool isMultiLang { get; set; } = true;
        public string Value { get; set; }
    }
}
