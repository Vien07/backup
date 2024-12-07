
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.HeaderPage.Models
{
    public class HeaderPageModelEdit : Database.HeaderPage
    {
        public string CheckBox { get; set; }

        public Database.HeaderPage GetDatabaseModel()
        {
            if (this.CheckBox == "on")
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = false;
            }
            Database.HeaderPage rs = new Database.HeaderPage();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Title = this.Title;
            rs.HeaderBlock = this.HeaderBlock;

            return rs;
        }

    }
    public class HeaderPageViewModel
    {



    }

    public class HeaderPageDetail
    {
        public Database.HeaderPage Detail { get; set; } = new Database.HeaderPage();

        //public Database.HeaderPage_Cate Cate { get; set; }


    }

}
