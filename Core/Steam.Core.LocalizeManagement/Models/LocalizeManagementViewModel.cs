
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Steam.Core.LocalizeManagement.Models
{
    public class LocalizeManagementModelEdit : Database.LocalizeManagement
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string fileStatus { get; set; }
        public string MediaPath { get; set; }
        public string KeyType { get; set; }
        public Database.LocalizeManagement GetDatabaseModel()
        {

            Database.LocalizeManagement rs = new Database.LocalizeManagement();
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
    public class LocalizeManagementViewModel
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

    public class LocalizeManagementDetail
    {
        public Database.LocalizeManagement Detail { get; set; } = new Database.LocalizeManagement();

        //public Database.LocalizeManagement_Cate Cate { get; set; }


    }

}
