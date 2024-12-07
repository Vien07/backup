
using Steam.Core.Base.Models;
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace Admin.HeaderPage.Models
{

    public class MenuModelEdit : Database.Menu
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string fileStatus { get; set; }
        public string FilePath { get; set; }
        public Database.Menu GetDatabaseModel()
        {

            Database.Menu rs = new Database.Menu();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Title = this.Title;
            rs.URL = this.URL;
            rs.ShowLevel = this.ShowLevel;
            rs.Icon = this.Icon;
            rs.Event = this.Event;
            rs.ParentID = this.ParentID;
            rs.RootParentID = this.RootParentID;
            rs.Description = this.Description;
            rs.MenuStylePid = this.MenuStylePid;

            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            return rs;
        }

    }
    public class MenuViewModel
    {
        public List<Database.Menu> Data { get; set; } = new List<Database.Menu>();
        public int Level { get; set; } = 0;



    }


    public class MenuDetail
    {
        public List<Database.Menu_Files> ListFiles { get; set; } = new List<Database.Menu_Files>();
        public Database.Menu Detail { get; set; } = new Database.Menu();

        //public Database.Menu_Cate Cate { get; set; }


    }

}
