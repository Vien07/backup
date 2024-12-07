
using Admin.EmailManagement.Database;
using ComponentUILibrary.Models;
using Steam.Core.Utilities.SteamModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.EmailManagement.Models
{
    public class EmailMailBoxViewModel
    {
        public List<MultilangModel> Languages { get; set; }
        public List<Data> Data { get; set; } = new List<Data>();
        public IPagedList<Data> List { get; set; }

        public string ActiveLang { get; set; }
    }

    public class EmailMailBoxDetail
    {
        public Database.EmailMailBox Detail { get; set; } = new Database.EmailMailBox();
    }

}
