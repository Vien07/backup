using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Product.Models
{
    public class Images_Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ProductDetailPid { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public string Images { get; set; } 
        public long Order { get; set; }
        public virtual ICollection<MultiLang_Images_Product> MultiLang_Images_Productes { get; set; }

    }
}
