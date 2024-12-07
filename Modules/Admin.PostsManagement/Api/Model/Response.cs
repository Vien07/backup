
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.PostsManagement.Api.Models.Response
{
    public class GetAllListSEOActive
    {

        public string PostSlug { get; set; }
        public string CateSlug { get; set; }
        public long PostPid { get; set; }
        public long? CatePid { get; set; }
    }
    public class GetListPostBySlug
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public string Images_Alt { get; set; }
        public string Slug { get; set; }
        public string ImagesPath { get; set; }
        public string CateSlug { get; set; }
        public string PublishDate { get; set; }
    }
    public class Post_List
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public List<Post_Item> Items { get; set; }
    }
    public class Post_Item
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public string Images_Alt { get; set; }
        public string Slug { get; set; }
        public string ImagesPath { get; set; }
        public string CateSlug { get; set; }
        public string PublishDate { get; set; }
    }

}
