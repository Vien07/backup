
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Steam.Core.Base.Models;

namespace Admin.PostsManagement.Database
{
    public class PostsManagement_Files : BaseEntity
    {

        public long PostsManagementId { get; set; }

        public string UploadFileName { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string? Caption { get; set; }
        public string? Description { get; set; }
        public string? Alt { get; set; }


    }
}
