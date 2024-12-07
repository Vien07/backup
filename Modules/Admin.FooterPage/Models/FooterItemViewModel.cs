
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.FooterPage.Models
{
    public class FooterItemModelEdit : Database.FooterItem
    {

        public Database.FooterItem GetDatabaseModel()
        {

            Database.FooterItem rs = new Database.FooterItem();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.ItemBlock = this.ItemBlock;

            return rs;
        }

    }
    public class FooterItemViewModel
    {



    }

    public class FooterItemDetail
    {
        public Database.FooterItem Detail { get; set; } = new Database.FooterItem();

        //public Database.FooterPage_Cate Cate { get; set; }


    }

}
