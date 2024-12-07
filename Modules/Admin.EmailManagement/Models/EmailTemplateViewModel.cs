
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.EmailManagement.Models
{
    public class EmailTemplateViewModel
    {
        public List<MultilangModel> Languages { get; set; }
        public List<Data> Data { get; set; } = new List<Data>();
        public IPagedList<Data> List { get; set; }

        public string ActiveLang { get; set; }


    }

    public class EmailTemplateDetail
    {
        public Database.EmailTemplate Detail { get; set; } = new Database.EmailTemplate();
    }

}
