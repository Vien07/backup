
using Steam.Core.Base.Models;
using ComponentUILibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace Admin.PostsCategory.Models
{


    public class PostsCategoryModelEdit : Database.PostsCategory
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile file_Thumbnail { get; set; }
        public string FileStatus_Thumbnail { get; set; }
        public string FilePath_Thumbnail { get; set; }   

        public IFormFile file_Banner { get; set; }
        public string FileStatus_Banner { get; set; }
        public string FilePath_Banner { get; set; }
        public Database.PostsCategory GetDatabaseModel()
        {

            Database.PostsCategory rs = new Database.PostsCategory();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Title = this.Title;
            rs.URL = this.URL;
            rs.ShowLevel = this.ShowLevel;
            rs.Icon = this.Icon;
            rs.Event = this.Event;
            rs.Slug = this.Slug;
            rs.WebsiteCatePage = this.WebsiteCatePage??"";
            rs.WebsiteDetailPage = this.WebsiteDetailPage??"";
            rs.Images_Alt = this.Images_Alt;
            rs.ParentID = this.ParentID;
            rs.RootParentID = this.RootParentID;
            rs.Description = this.Description;
            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            return rs;
        }

    }
    public class PostsCategoryViewModel
    {
        public List<PostsCategory_Item> Data { get; set; } = new List<PostsCategory_Item>();
        public int Level { get; set; } = 0;



    }
    public class PostsCategory_List 
    {

        public List<PostsCategory_Item> Items { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
    public class PostsCategory_Item: Database.PostsCategory
    {

        public string WebsiteSlug { get; set; }
        public string FullPathBanner { get; set; }
        public string FullThumbnail { get; set; }
        public string Slug { get; set; }
        public string Cate { get; set; }
        public string CateSlug { get; set; }
        public string ImagePath { get; set; }
    }    

    public class PostsCategoryDetail
    {
        public List<Database.PostsCategory_Files> ListFiles { get; set; } = new List<Database.PostsCategory_Files>();
        public Database.PostsCategory Detail { get; set; } = new Database.PostsCategory();

        //public Database.PostsCategory_Cate Cate { get; set; }


    }

}
