
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Steam.Core.Base.Models;

namespace Admin.Sample.Database
{
    public class Sample_Files: BaseEntity
    {

        public long SampleId { get; set; }

        public string UploadFileName { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }


    }
}
