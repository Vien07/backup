using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Feature.Models
{
    public class FeatureCate_FeatureDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long FeatureDetailPid { get; set; }
        public virtual FeatureDetail FeatureDetail { get; set; }
        public long FeatureCatePid { get; set; }
        public virtual FeatureCate FeatureCate { get; set; }
    }
}
