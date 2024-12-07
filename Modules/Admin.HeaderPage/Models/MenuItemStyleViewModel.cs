
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.HeaderPage.Models
{
    public class MenuItemStyleModelEdit : Database.MenuItemStyle
    {

        public Database.MenuItemStyle GetDatabaseModel()
        {

            Database.MenuItemStyle rs = new Database.MenuItemStyle();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Level = this.Level;
            rs.ItemBlock = this.ItemBlock;

            return rs;
        }

    }
    public class MenuItemStyleViewModel
    {



    }

    public class MenuItemStyleDetail
    {
        public Database.MenuItemStyle Detail { get; set; } = new Database.MenuItemStyle();

        //public Database.HeaderPage_Cate Cate { get; set; }


    }

}
