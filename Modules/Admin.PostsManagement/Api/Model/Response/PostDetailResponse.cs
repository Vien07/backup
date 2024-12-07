
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.PostsManagement.Api.Models.Response
{

    public class PostDetailResponse
    {

        public string Slug { get; set; }
        public string Meta { get; set; }
        public string CateSlug { get; set; }
        public string ImagesPath { get; set; }
        public string Title { get; set; }
        public string? PublishDate { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public string? Images_Caption { get; set; }
        public string? Images_Description { get; set; }
        public string? Images_Alt { get; set; }
        public string? Author { get; set; }
        public string? LinkAuthor { get; set; }
        public string? TableOfContent { get; set; }
        public string? SeeMore { get; set; }
    }
}
