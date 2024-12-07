
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.PostsManagement.Api.Models.Request
{
    public class GetPostBySlugRequest
    {

        public string PostSlug { get; set; } = String.Empty;
    }
}
