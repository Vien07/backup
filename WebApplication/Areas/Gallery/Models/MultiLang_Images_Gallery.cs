using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Gallery.Models
{
    public class MultiLang_Images_Gallery
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long ImagesGalleryPid { get; set; }
        public virtual Images_Gallery ImagesGallery { get; set; }
        public string Caption { get; set; }
        public string LangKey { get; set; }
    } 
}
