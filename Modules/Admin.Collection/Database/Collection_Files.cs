
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Steam.Core.Base.Models;

namespace Admin.Collection.Database
{
    public class Collection_Files : BaseEntity
    {

        public long CollectionId { get; set; }

        public string UploadFileName { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string Caption { get; set; }
        public string Description { get; set; }


    }
}
