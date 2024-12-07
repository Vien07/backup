
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.MemberManagement.Models
{

    public class Data
    {
        public int Pid { get; set; }
        public string Title { get; set; }

        public string? Description { get; set; } = String.Empty;
        public string Images { get; set; } = String.Empty;
        public string? Link { get; set; } = "";
        public string? Position { get; set; } = String.Empty;
        public double Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;

        public DateTime UpdateDate { get; set; }
        public string Lang { get; set; }

    }

    public class MemberManagementDetail
    {
        public List<Database.MemberManagement_Files> ListFiles { get; set; } = new List<Database.MemberManagement_Files>();
        public Database.MemberManagement Detail { get; set; } = new Database.MemberManagement();

        //public Database.MemberManagement_Cate Cate { get; set; }


    }

}
