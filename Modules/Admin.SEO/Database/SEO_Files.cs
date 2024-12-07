
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Steam.Core.Base.Models;

namespace Admin.SEO.Database
{
    public class SEO_Files: BaseEntity
    {

        public long SEOId { get; set; }

        public string UploadFileName { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }


    }
}
