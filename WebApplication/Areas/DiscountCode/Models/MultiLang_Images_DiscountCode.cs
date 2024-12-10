using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.DiscountCode.Models
{
    public class MultiLang_Images_DiscountCode
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ImagesDiscountCodePid { get; set; }
        public virtual Images_DiscountCode ImagesDiscountCode { get; set; }
        public string Caption { get; set; }
        public string LangKey { get; set; }
    } 
}
