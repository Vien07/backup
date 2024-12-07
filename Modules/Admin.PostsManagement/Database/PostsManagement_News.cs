
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Steam.Core.Base.Constant;
using Steam.Core.Base.Models;

namespace Admin.PostsManagement.Database
{
    public partial class PostsManagement 
    {
        public string? TableOfContent { get; set; }
        public string? SeeMore { get; set; }
        public string? Author { get; set; } = "ofd";
        public string? LinkAuthor { get; set; } = "/";
    }

}

