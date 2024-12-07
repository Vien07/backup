
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.Slider.Models
{
    public class SliderModelEdit : Database.Slider
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string fileStatus { get; set; }
        public string FilePath { get; set; }
        public Database.Slider GetDatabaseModel()
        {

            Database.Slider rs = new Database.Slider();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Title = this.Title;

            rs.Description = this.Description;
   
            rs.Images = this.Images;


            rs.Link = this.Link;
            rs.Position = this.Position;

            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            return rs;
        }

    }
    public class SliderViewModel
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

    public class SliderDetail
    {
        public Database.Slider Detail { get; set; } = new Database.Slider();

        //public Database.Slider_Cate Cate { get; set; }


    }

}
