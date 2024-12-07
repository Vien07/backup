
using ComponentUILibrary.Models;
using Steam.Core.Common.SteamString;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;

namespace Admin.SEO.Models
{
    public class SEOIntegrateModelEdit : Database.SEO
    {
        public long SEOPid { get; set; }
        public long CatePid { get; set; }
        public string ListFiles { get; set; }
        public string PageTitle { get; set; }
        public string OgImage { get; set; }
        public Database.SEO GetDatabaseModel()
        {
            Database.SEO rs = new Database.SEO();

            rs.Description = this.Description;
            rs.Images = this.Images;
            rs.PostSlug = this.PostSlug.RemoveSign4VietnameseString();
            rs.PostTitle = this.PostTitle;
            rs.TagKeys = this.TagKeys;
            rs.MetaDescription = this.MetaDescription;
            rs.Meta = this.Meta;
            rs.PostPid = this.PostPid;
            rs.ModuleCode = this.ModuleCode;
            rs.ExtraMeta = this.ExtraMeta;
            rs.OgType = this.OgType;

            return rs;
        }

    }
    public class SaveModel : Database.SEO
    {
        public long SEOPid { get; set; }
        public long CatePid { get; set; }
        public string ListFiles { get; set; }
        public string PageTitle { get; set; }
        public string OgImage { get; set; }
        public Database.SEO GetDatabaseModel()
        {
            Database.SEO rs = new Database.SEO();

            rs.Description = this.Description;
            rs.Images = this.Images;
            rs.PostSlug = this.PostSlug.RemoveSign4VietnameseString();
            rs.PostTitle = this.PostTitle;
            rs.TagKeys = this.TagKeys;
            rs.MetaDescription = this.MetaDescription;
            rs.Meta = this.Meta;
            rs.PostPid = this.PostPid;
            rs.ModuleCode = this.ModuleCode;
            rs.ExtraMeta = this.ExtraMeta;
            rs.OgType = this.OgType;

            return rs;
        }

    }
    public class SEOViewModel
    {
        public List<MultilangModel> Languages { get; set; }
        public List<Data> Data { get; set; } = new List<Data>();
        public IPagedList<Data> List { get; set; }

        public string ActiveLang { get; set; }


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

    public class SEODetail
    {
        public List<Database.SEO_Files> ListFiles { get; set; } = new List<Database.SEO_Files>();
        public Database.SEO Detail { get; set; } = new Database.SEO();

        //public Database.SEO_Cate Cate { get; set; }


    }
    public class SEO_List
    {


        public List<SEO_Item> Items { get; set; } = new List<SEO_Item>();
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class SEO_Item : Database.SEO
        {

        }
    }

}
