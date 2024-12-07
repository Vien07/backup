
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.LayoutPage.Models
{
    public class MenuStyleModelEdit : Database.MenuStyle
    {
        public string CheckBox { get; set; }

        public Database.MenuStyle GetDatabaseModel()
        {
            if (this.CheckBox == "on")
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = false;
            }
            Database.MenuStyle rs = new Database.MenuStyle();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Title = this.Title;
            rs.MenuBlock = this.MenuBlock;

            return rs;
        }

    }
    public class MenuStyleViewModel
    {



    }

    public class MenuStyleDetail
    {
        public Database.MenuStyle Detail { get; set; } = new Database.MenuStyle();

        //public Database.HeaderPage_Cate Cate { get; set; }


    }

}
