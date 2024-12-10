using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.News.Models
{
    public class NewsCate_NewsDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long NewsDetailPid { get; set; }
        public virtual NewsDetail NewsDetail { get; set; }
        public long NewsCatePid { get; set; }
        public virtual NewsCate NewsCate { get; set; }
    }
}
