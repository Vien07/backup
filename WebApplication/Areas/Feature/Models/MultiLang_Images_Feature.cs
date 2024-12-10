using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Feature.Models
{
    public class MultiLang_Images_Feature
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ImagesFeaturePid { get; set; }
        public virtual Images_Feature ImagesFeature { get; set; }
        public string Caption { get; set; }
        public string LangKey { get; set; }
    } 
}
