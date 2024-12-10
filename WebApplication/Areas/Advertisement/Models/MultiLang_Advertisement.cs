using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Advertisement.Models
{
    public class MultiLang_Advertisement
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public int AdvertisementPid { get; set; }
        public virtual Advertisement Advertisement { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string EmbedCode { get; set; }
        public string LangKey { get; set; }
    }
}
