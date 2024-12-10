using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Promotion.Models
{
    public class MultiLang_Images_Promotion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ImagesPromotionPid { get; set; }
        public virtual Images_Promotion ImagesPromotion { get; set; }
        public string Caption { get; set; }
        public string LangKey { get; set; }
    } 
}
