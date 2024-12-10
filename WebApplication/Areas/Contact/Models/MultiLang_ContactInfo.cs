using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Contact.Models
{
    public class MultiLang_ContactInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }

        public long ContactInfoID { get; set; }
        public virtual ContactInfo ContactInfo { get; set; }

        public string Value { get; set; }
        public string LangKey { get; set; }
    }
}
