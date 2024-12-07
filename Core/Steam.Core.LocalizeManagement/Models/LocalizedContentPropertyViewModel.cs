
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Steam.Core.LocalizeManagement.Models
{
    public class LocalizedContentPropertyModelEdit : Database.LocalizedContentProperty
    {

        public Database.LocalizedContentProperty GetDatabaseModel()
        {

            Database.LocalizedContentProperty rs = new Database.LocalizedContentProperty();
            rs.Pid = this.Pid;
        
          


            return rs;
        }

    }
    public class LocalizedContentPropertyViewModel
    {
        public List<MultilangModel> Languages { get; set; }
        public List<Data> Data { get; set; } = new List<Data>();
        public IPagedList<Data> List { get; set; }

        public string ActiveLang { get; set; }


    }

    public class LocalizedContentPropertyDetail
    {
        public Database.LocalizedContentProperty Detail { get; set; } = new Database.LocalizedContentProperty();

        //public Database.LocalizedContentProperty_Cate Cate { get; set; }


    }

}
