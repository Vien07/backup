
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.Authorization.Models
{
    public class LogManagementModelEdit : Database.LogManagement
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string fileStatus { get; set; }
        public string FilePath { get; set; }
        public Database.LogManagement GetDatabaseModel()
        {

            Database.LogManagement rs = new Database.LogManagement();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.ActionName = this.ActionName;

            rs.ActionData = this.ActionData;
   
            rs.ActionUrl = this.ActionUrl;


            rs.Status = this.Status;



            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            return rs;
        }

    }
    public class LogManagementViewModel
    {
        public List<MultilangModel> Languages { get; set; }
        public List<Data> Data { get; set; } = new List<Data>();
        public IPagedList<Data> List { get; set; }

        public string ActiveLang { get; set; }


    }
    public class DataLogManagement
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

    public class LogManagementDetail
    {
        public Database.LogManagement Detail { get; set; } = new Database.LogManagement();

        //public Database.LogManagement_Cate Cate { get; set; }


    }

}
