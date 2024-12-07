
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.PostsManagement.Api.Models.Request
{
  
    public class GetListPostBySlug
    {

        public string RootSlug { get; set; }=String.Empty;
        public string? CateSlug { get; set; }=String.Empty;
        public int PageIndex { get; set; }=1;
        public int PageSize { get; set; }=10;
    }  
    public class GetListNewPostByCateSlug
    {

        public string RootSlug { get; set; }=String.Empty;
        public string? CateSlug { get; set; }=String.Empty;
        public int TakeItem { get; set; } = 8;

    }
    public class GetListRelatePostByPostSlug
    {

        public string RootSlug { get; set; }=String.Empty;
        public string? PostSlug { get; set; }=String.Empty;
        public int TakeItem { get; set; }=8;
    }
}
