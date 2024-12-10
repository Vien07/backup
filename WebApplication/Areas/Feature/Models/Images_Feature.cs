using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Areas.Feature.Models
{
    public class Images_Feature
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long FeatureDetailPid { get; set; }
        public virtual FeatureDetail FeatureDetail { get; set; }
        public string Images { get; set; } 
        public long Order { get; set; }
        public virtual ICollection<MultiLang_Images_Feature> MultiLang_Images_Featurees { get; set; }

    }
}
