using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Configurations.Models
{
    public class EmailTempateVariable
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Group  { get; set; }
        public string Value  { get; set; }

    }
}
