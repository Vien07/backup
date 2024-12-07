
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.LayoutPage.Models
{
    public class QuickToolBarItemModelEdit : Database.QuickToolBarItem
    {

        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string fileStatus { get; set; }
        public string FilePath { get; set; }
        public Database.QuickToolBarItem GetDatabaseModel()
        {

            Database.QuickToolBarItem rs = new Database.QuickToolBarItem();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Title = this.Title;
            rs.QuickToolBarPid = this.QuickToolBarPid;


            rs.Icon = this.Icon;
            rs.Value = this.Value;
            rs.Key = this.Key;

            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            return rs;
        }

    }
    public class QuickToolBarItemViewModel
    {



    }

    public class QuickToolBarItemDetail
    {
        public Database.QuickToolBarItem Detail { get; set; } = new Database.QuickToolBarItem();

        //public Database.QuickToolBarPage_Cate Cate { get; set; }


    }

}
