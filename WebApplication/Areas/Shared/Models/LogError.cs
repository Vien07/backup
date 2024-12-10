using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Shared.Models
{
    public class LogError
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public string Trace { get; set; }
        public string Message { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
