
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.MemberManagement.Models
{
    public class FeedbackModelEdit : Database.Feedback
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public  IFormFile files { get; set; }
        public string FileStatus { get; set; }
        public string FilePath { get; set; }
        public string isNewCheckBox { get; set; }
        public Database.Feedback GetDatabaseModel()
        {
            Database.Feedback rs = new Database.Feedback();

            if (this.CheckBox == "on")
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = false;
            }    
            if (this.isNewCheckBox == "1")
            {
                rs.isNew = true;
            }
            else
            {
                rs.isNew = false;
            }
            rs.Pid = this.Pid;
            rs.Order = this.Order;
            rs.FullName = this.FullName;
            rs.Email = this.Email;
            rs.Rating = this.Rating;
            rs.SKU = this.SKU;
            //rs.PublishDate = this.PublishDate;

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
            return rs;
        }

    }
    public class Feedback_List
    {


        public List<Feedback_Item> Items { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


    }
    public class Feedback_Item : Database.Feedback
    {


        public string Slug { get; set; }
        public string Cate { get; set; }
        public string CateSlug { get; set; }


    }
    public class FeedbackModel
    {
        public class ParamSearch
        {

            public string KeySearch { get; set; } = String.Empty;
            public string Cate { get; set; } = "0";
            public string Type { get; set; } = "0";
            public string Active { get; set; } = "";
            public bool? isEnable { get; set; } 
            public int PageIndex { get; set; } = 1;
            public ParamSearch Init()
            {
                var rs = new ParamSearch();
                try
                {
                    rs.KeySearch = this.KeySearch;
                    rs.Cate = this.Cate;
                    rs.Type = this.Type;
                    if(this.Active == "0")
                    {
                        rs.isEnable = false;
                    }
                    else if (this.Active == "1")
                    {
                        rs.isEnable = true;
                    }
                    else
                    {
                        rs.isEnable = null;
                    }
                    if (this.Cate == null)
                    {
                        rs.Cate  = "0" ;
                    }
               
                }
                catch (Exception ex)
                {

                    throw;
                }
                return rs;
            }
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

        public class FeedbackDetail
        {
            public List<Database.Feedback_Files> ListFiles { get; set; } = new List<Database.Feedback_Files>();
            public Database.Feedback Detail { get; set; } = new Database.Feedback();

            //public Database.Feedback_Cate Cate { get; set; }


        }
    }

}
