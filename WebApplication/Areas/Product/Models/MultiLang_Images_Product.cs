using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Product.Models
{
    public class MultiLang_Images_Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ImagesProductPid { get; set; }
        public virtual Images_Product ImagesProduct { get; set; }
        public string Caption { get; set; }
        public string LangKey { get; set; }
    } 
}
