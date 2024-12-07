
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.LayoutPage.Models
{
    public class SliderItemModelEdit : Database.SliderItem
    {

        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string fileStatus { get; set; }
        public string FilePath { get; set; }
        public Database.SliderItem GetDatabaseModel()
        {

            Database.SliderItem rs = new Database.SliderItem();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Title = this.Title;
            rs.SliderPid = this.SliderPid;

            rs.Description = this.Description;

            rs.Images = this.Images;
            rs.VideoLink = this.VideoLink;
            rs.TypeMedia = this.TypeMedia;
            if(this.TypeMedia == "video")
            {
                rs.VideoLink = this.VideoLink;

            }

            rs.Link = this.Link;
            rs.Position = this.Position;

            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            return rs;
        }

    }
    public class SliderItemViewModel
    {



    }

    public class SliderItemDetail
    {
        public Database.SliderItem Detail { get; set; } = new Database.SliderItem();

        //public Database.SliderPage_Cate Cate { get; set; }


    }

}
