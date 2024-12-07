
using Steam.Core.Base.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.MemberManagement.Database
{
    public class MemberManagement_Files: BaseEntity
    {

        public long MemberManagementId { get; set; }

        public string UploadFileName { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }


    }
}
