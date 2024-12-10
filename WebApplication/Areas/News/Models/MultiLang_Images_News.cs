using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.News.Models
{
    public class MultiLang_Images_News
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ImagesNewsPid { get; set; }
        public virtual Images_News ImagesNews { get; set; }
        public string Caption { get; set; }
        public string LangKey { get; set; }
    } 
}
