
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Steam.Core.Base.Models;

namespace Admin.MemberManagement.Database
{
    public class Feedback_Files : BaseEntity
    {

        public long FeedbackId { get; set; }

        public string UploadFileName { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string Caption { get; set; }
        public string Description { get; set; }


    }
}
