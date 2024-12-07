
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.WebsiteKeys.Models
{
    public class WebsiteKeysModelEdit : Database.WebsiteKeys
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string fileStatus { get; set; }
        public string MediaPath { get; set; }
        public string KeyType { get; set; }
        public Database.WebsiteKeys GetDatabaseModel()
        {

            Database.WebsiteKeys rs = new Database.WebsiteKeys();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Key = this.Key;

            rs.Description = this.Description;
   
            rs.Value = this.Value;
            rs.Title = this.Title;
            rs.Group = this.Group;


            rs.isSystemKey = this.isSystemKey;
            if(this.CheckBox == "0")
            {
                rs.isMedia = false;
            }
            else { 
            rs.isMedia = true;
            }
            if(rs.isMedia)
            {
                rs.MediaPath = this.MediaPath;
            }
            

            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            if (this.KeyType == "sys")
            {
                rs.isSystemKey = true;
            }
            else
            {
                rs.isSystemKey = false;
            }



            return rs;
        }

    }
    public class WebsiteKeysViewModel
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

    public class WebsiteKeysDetail
    {
        public Database.WebsiteKeys Detail { get; set; } = new Database.WebsiteKeys();

        //public Database.WebsiteKeys_Cate Cate { get; set; }


    }

}
