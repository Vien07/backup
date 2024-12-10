using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Calendar.Models
{
    public class MultiLang_Calendar
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CalendarPid { get; set; }
        public virtual Calendar Calendar { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string LangKey { get; set; }
    }
}
