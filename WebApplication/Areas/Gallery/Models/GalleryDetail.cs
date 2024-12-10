using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Gallery.Models
{
    public class GalleryDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        [Required(ErrorMessage = "Required")]
        public string PicThumb { get; set; } = string.Empty;
        public string TagKey { get; set; }
        public string SlugTagKey { get; set; }
        public string VideoLink { get; set; }
        public DateTime PublishDate { get; set; }
        public long CounterView { get; set; } = 0;
        public long Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool IsHot { get; set; } = false;
        public bool Deleted { get; set; } = false;

        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public virtual ICollection<MultiLang_GalleryDetail> MultiLang_GalleryDetails { get; set; }
        public virtual ICollection<Images_Gallery> Images_Galleryes { get; set; }
        public virtual ICollection<GalleryCate_GalleryDetail> GalleryCate_GalleryDetails { get; set; }
    }
}
