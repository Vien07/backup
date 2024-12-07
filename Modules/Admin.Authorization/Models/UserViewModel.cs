
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.Authorization.Models
{
    public class UserViewModel
    {
        public List<MultilangModel> Languages { get; set; }
        public List<Data> Data { get; set; } = new List<Data>();
        public IPagedList<Data> List { get; set; }

        public string ActiveLang { get; set; }


    }
    public class UserDetail
    {
        public Database.User Detail { get; set; } = new Database.User();

        //public Database.Sample_Cate Cate { get; set; }


    }
}
