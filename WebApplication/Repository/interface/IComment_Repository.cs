using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Areas.Comment.Models;
using DTO.Comment;

namespace CMS.Repository
{
    public interface IComment_Repository
    {
        Task<List<CommentDto>> GetList(string lang, int page);
    }
}