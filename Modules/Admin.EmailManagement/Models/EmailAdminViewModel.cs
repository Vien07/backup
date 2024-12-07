
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.EmailManagement.Models
{
    public class EmailAdminViewModel
    {
        public List<MultilangModel> Languages { get; set; }
        public List<Data> Data { get; set; } = new List<Data>();
        public IPagedList<Data> List { get; set; }

        public string ActiveLang { get; set; }


    }
    public class Data
    {
        public int Pid { get; set; }
        public string Title { get; set; }

        public string? Content { get; set; } = String.Empty;
        public double Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;

        public DateTime UpdateDate { get; set; }
        public string Lang { get; set; }

    }

    public class EmailAdminDetail
    {
        public Database.EmailAdmin Detail { get; set; } = new Database.EmailAdmin();
    }

}
