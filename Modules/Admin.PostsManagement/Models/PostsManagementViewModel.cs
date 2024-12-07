
using ComponentUILibrary.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;
using Admin.SEO.Database;
using ComponentUILibrary.ViewComponents;
using Admin.PostsManagement.Constants;
using Steam.Core.Common.SteamString;

namespace Admin.PostsManagement.Models
{
    public class SaveModel : Database.PostsManagement
    {
        public string CheckBox { get; set; }
        public string ListFiles { get; set; }
        public IFormFile files { get; set; }
        public string FileStatus { get; set; }
        public string FilePath { get; set; }
        public string isNewCheckBox { get; set; }
        public Database.PostsManagement GetDatabaseModel()
        {

            Database.PostsManagement rs = new Database.PostsManagement();

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
            rs.Title = this.Title;
            rs.PublishDate = this.PublishDate;
            rs.Description = this.Description;
            rs.Content = this.Content;
            rs.Images = this.Images;
            rs.FilePath = this.FilePath;
            rs.Images_Caption = this.Images_Caption;
            rs.TableOfContent = this.TableOfContent;
            rs.LastLogin = this.LastLogin;
            rs.Images_Description = this.Images_Description;
            rs.Images_Alt = this.Images_Alt;
            rs.Link = this.Link;
            rs.Position = this.Position;
            rs.CateID = this.CateID;
            rs.SubCate = this.SubCate;
            rs.SeeMore = this.SeeMore;
            rs.Author = this.Author;
            rs.TypePost = this.TypePost;
            rs.Group = this.Group;
            rs.LinkAuthor = this.LinkAuthor;

            rs.CreateDate = DateTime.Now;
            rs.UpdateDate = DateTime.Now;
            rs.Enabled = this.Enabled;

            return rs;
        }
        public Database.PostsManagement SaveImages(Dictionary<string, string> config, Database.PostsManagement rs )
        {
            FileUploadFilePondComponent filePond = new FileUploadFilePondComponent();
            DropzoneComponent dropzone = new DropzoneComponent();


            rs.Pid = this.Pid;
            rs.Order = this.Order;
                      #region image
            var image = filePond.SaveImage(new FileUploadControlModel.SaveImageModel
            {
                Title = this.Title.ToSlug(),
                File = this.files,
                FilePath = this.FilePath,
                UploadThumbPath = PostsManagementConstants.StaticPath.Asset.ImageThumb,
                UploadPath = PostsManagementConstants.StaticPath.Asset.Image,
                Filestatus = this.FileStatus,
                Height = Convert.ToInt32(config[PostsManagementConstants.Config.Admin.MaxHeight].ToString()),
                Width = Convert.ToInt32(config[PostsManagementConstants.Config.Admin.MaxWidth].ToString()),
            });
            if (!image.IsExisted)
            {
                rs.Images = image.ImageName;
                rs.FilePath = image.FilePath;
            }
            #endregion
            #region ListImages 
            if (this.ListFiles != null)
            {
                this.ListImages = dropzone.SaveListFile(new DropzoneModel.SaveImageModel
                {
                    UploadThumbPath = PostsManagementConstants.StaticPath.Asset.ImageThumb,
                    UploadPath = PostsManagementConstants.StaticPath.Asset.Image,
                    ImageName = this.Title.ToSlug(),
                    Height = Convert.ToInt32(config[PostsManagementConstants.Config.Admin.MaxHeight].ToString()),
                    Width = Convert.ToInt32(config[PostsManagementConstants.Config.Admin.MaxWidth].ToString())
                }, this.ListFiles);

            }

            #endregion
            return rs;
        }
        public List<DropzoneModel.File> ListImages { get; set; }
    }
    public class PostsManagement_List
    {


        public List<PostsManagement_Item> Items { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


    }
    public class PostsManagement_Item : Database.PostsManagement
    {


        public string Slug { get; set; }
        public string Cate { get; set; }
        public string CateSlug { get; set; }
        public string ImagePath { get; set; }


    }
    public class PostsManagementModel
    {
        public class ParamSearch
        {

            public string KeySearch { get; set; } = String.Empty;
            public string Cate { get; set; } = "0";
            public string Type { get; set; } = "0";
            public string Group { get; set; } = "Post";
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
                    if (this.Active == "0")
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
                        rs.Cate = "0";
                    }
                    rs.Group = Group;


                }
                catch (Exception ex)
                {

                    throw;
                }
                return rs;
            }
        }


        public class PostsManagementDetail
        {
            public List<Database.PostsManagement_Files> ListFiles { get; set; } = new List<Database.PostsManagement_Files>();
            public Database.PostsManagement Detail { get; set; } = new Database.PostsManagement();

            //public Database.PostsManagement_Cate Cate { get; set; }


        }
    }

}
