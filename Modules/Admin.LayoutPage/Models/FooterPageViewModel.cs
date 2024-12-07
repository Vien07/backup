
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.LayoutPage.Models
{
    public class FooterPageModelEdit : Database.FooterPage
    {
        public string CheckBox { get; set; }

        public Database.FooterPage GetDatabaseModel()
        {
            if (this.CheckBox == "on")
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = false;
            }
            Database.FooterPage rs = new Database.FooterPage();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Name = this.Name;
            rs.FooterBlock = this.FooterBlock;

            return rs;
        }

    }
    public class FooterPageViewModel
    {



    }

    public class FooterPageDetail
    {
        public Database.FooterPage Detail { get; set; } = new Database.FooterPage();

        //public Database.FooterPage_Cate Cate { get; set; }


    }

}
