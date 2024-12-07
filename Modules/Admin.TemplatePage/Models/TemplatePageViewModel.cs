
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.TemplatePage.Models
{
    public class TemplatePageModelEdit : Database.TemplatePage
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string fileStatus { get; set; }
        public string FilePath { get; set; }
        public Database.TemplatePage GetDatabaseModel()
        {

            Database.TemplatePage rs = new Database.TemplatePage();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Name = this.Name;

            rs.Description = this.Description;
   
            rs.Url = this.Url;


            rs.PageType = this.PageType;
            rs.Parameters = this.Parameters;


            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            return rs;
        }

    }
    public class TemplatePageViewModel
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

    public class TemplatePageDetail
    {
        public Database.TemplatePage Detail { get; set; } = new Database.TemplatePage();

        //public Database.TemplatePage_Cate Cate { get; set; }


    }  
    public class TemplatePageMeta
    {
        public string Meta { get; set; }=String.Empty;



    }

}
