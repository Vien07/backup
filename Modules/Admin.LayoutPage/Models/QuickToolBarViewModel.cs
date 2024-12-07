
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.LayoutPage.Models
{
    public class QuickToolBarModelEdit : Database.QuickToolBar
    {
        public string CheckBox { get; set; }

        public Database.QuickToolBar GetDatabaseModel()
        {
            if (this.CheckBox == "on")
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = false;
            }
            Database.QuickToolBar rs = new Database.QuickToolBar();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Name = this.Name;
            rs.QuickToolBarBlock = this.QuickToolBarBlock;

            return rs;
        }

    }
    public class QuickToolBarViewModel
    {



    }

    public class QuickToolBarDetail
    {
        public Database.QuickToolBar Detail { get; set; } = new Database.QuickToolBar();

        //public Database.QuickToolBar_Cate Cate { get; set; }


    }

}
