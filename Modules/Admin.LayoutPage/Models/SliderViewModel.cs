
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.LayoutPage.Models
{
    public class SliderModelEdit : Database.Slider
    {
        public string CheckBox { get; set; }

        public Database.Slider GetDatabaseModel()
        {
            if (this.CheckBox == "on")
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = false;
            }
            Database.Slider rs = new Database.Slider();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Name = this.Name;
            rs.SliderBlock = this.SliderBlock;

            return rs;
        }

    }
    public class SliderViewModel
    {



    }

    public class SliderDetail
    {
        public Database.Slider Detail { get; set; } = new Database.Slider();

        //public Database.Slider_Cate Cate { get; set; }


    }

}
