
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.LayoutPage.Models
{
    
    public class HomePageModelEdit : Database.HomePage
    {
        public string CheckBox { get; set; }
        
        public Database.HomePage GetDatabaseModel()
        {
            if (this.CheckBox == "on")
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = false;
            }
            Database.HomePage rs = new Database.HomePage();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Title = this.Title;
            rs.Name = this.Name;
            rs.Section = this.Section;
            rs.TypeView = this.TypeView;
            rs.ListItemHtml = this.ListItemHtml;
            rs.ListItemChildHtml = this.ListItemChildHtml;
            rs.ListTabHtml = this.ListTabHtml;
            rs.ScriptBlock = this.ScriptBlock;
            rs.StyleBlock = this.StyleBlock;
            rs.SourceData = this.SourceData;         
            return rs;
        }

    }
    public class HomePageModel
    {
        public class HomePageViewModel
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
            public string Name { get; set; }
            public string Section { get; set; }
            public string SourceData { get; set; }
            public string? ListItemHTML { get; set; } = "";

            public double Order { get; set; }
            public bool Enabled { get; set; } = true;
            public bool Deleted { get; set; } = false;

            public DateTime UpdateDate { get; set; }
            public string Lang { get; set; }

        }

        public class HomePageDetail
        {
            public Database.HomePage Detail { get; set; } = new Database.HomePage();

            //public Database.HomePage_Cate Cate { get; set; }


        }
        public class ParamSearch
        {

            public string KeySearch { get; set; } = String.Empty;
            public string Cate { get; set; } = "0";
            public string Type { get; set; } = "0";
            public int PageIndex { get; set; } = 1;
        }
    }
}