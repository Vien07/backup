
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;
using Admin.SEO.Database;

namespace Admin.Collection.Models
{
    public class CollectionModelEdit : Database.Collection
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string FileStatus { get; set; }
        public string FilePath { get; set; }
        public Database.Collection GetDatabaseModel()
        {
            if (this.CheckBox == "on")
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = false;
            }
            Database.Collection rs = new Database.Collection();
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.Title = this.Title;
            rs.PublishDate = this.PublishDate;
            rs.Description = this.Description;
            rs.Content = this.Content;
            rs.Images = this.Images;
            rs.FilePath = this.FilePath;
            rs.Images_Caption = this.Images_Caption;
            rs.LastLogin = this.LastLogin;
            rs.Images_Description = this.Images_Description;
            rs.Images_Alt = this.Images_Alt;
            rs.Link = this.Link;
            rs.Position = this.Position;
            rs.CateID = this.CateID;
            rs.SubCate = this.SubCate;
            rs.CreateUser = this.CreateUser;
            rs.UpdateUser = this.UpdateUser;
            rs.ProductIDs = this.ProductIDs;
            return rs;
        }

    }
    public class Collection_Product_List
    {
    
        public List<Collection_Product_Item> ProductItems { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public class ParamSearch
        {

            public string KeySearch { get; set; } = String.Empty;
            public string ChoosenProducts { get; set; } = String.Empty;
            public string CateIds { get; set; } = "";
            public int PageIndex { get; set; } = 1;
        }
    }

    public class Collection_Product_Item
    {
        public long Pid { get; set; }
        public string SKU { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string RootSlug { get; set; }
        public string Price { get; set; }
        public string Images { get; set; }
        public string ImagePath { get; set; }
        public double Order { get; set; }


    }
    public class Collection_List
    {


        public List<Collection_Item> Items { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


    }
    public class Collection_Item : Database.Collection
    {


        public string Slug { get; set; }
        public string Cate { get; set; }
        public string ImagePath { get; set; }


    }

    public class CollectionModel
    {
        public class ParamSearch
        {

            public string KeySearch { get; set; } = String.Empty;
            public string Cate { get; set; } = "0";
            public string Type { get; set; } = "0";
            public int PageIndex { get; set; } = 1;
        }

        public class Data
        {
            public int Pid { get; set; }
            public string Title { get; set; }

            public string? Description { get; set; } = String.Empty;
            public string Images { get; set; } = String.Empty;
            public string? Link { get; set; } = "";
            public string? Position { get; set; } = String.Empty;
            public double Order { get; set; }
            public bool Enabled { get; set; } = true;
            public bool Deleted { get; set; } = false;

            public DateTime UpdateDate { get; set; }
            public string Lang { get; set; }

        }

        public class CollectionDetail
        {
            public List<Database.Collection_Files> ListFiles { get; set; } = new List<Database.Collection_Files>();
            public Database.Collection Detail { get; set; } = new Database.Collection();

            //public Database.Collection_Cate Cate { get; set; }


        }
    }

}
