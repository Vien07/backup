using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.News.Models
{
    public class Images_News
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long NewsDetailPid { get; set; }
        public virtual NewsDetail NewsDetail { get; set; }
        public string Images { get; set; } 
        public long Order { get; set; }
        public virtual ICollection<MultiLang_Images_News> MultiLang_Images_Newses { get; set; }

    }
}
