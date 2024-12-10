using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Gallery.Models
{
    public class GalleryCate_GalleryDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long GalleryDetailPid { get; set; }
        public virtual GalleryDetail GalleryDetail { get; set; }
        public long GalleryCatePid { get; set; }
        public virtual GalleryCate GalleryCate { get; set; }
    }
}
