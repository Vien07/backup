using CMS.Areas.Comment.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DTO.Common;

namespace CMS.Areas.Comment
{
    public interface ICommentRepository
    {
        dynamic LoadData(SearchDto search);
        bool Enable(long[] Pid, bool active);
        dynamic Delete(int Pid);
        dynamic DeleteAll(int Pid);
        dynamic Delete(long[] Pid);
        dynamic Edit(long Pid);
        dynamic Insert(Models.Comment Comment, List<MultiLang_Comment> multiLang_Comment, IFormFile Images, IFormFile PicThumb2);
        dynamic Update(Models.Comment Comment, List<MultiLang_Comment> multiLang_Comment, IFormFile images, IFormFile PicThumb2);
        bool Count(int code);
        bool Coppy(long[] Pid);
        dynamic Validation(dynamic data);
        bool Up(long Pid);
        bool Down(long Pid);
        bool UpdateOrder(long Pid, int order);
        bool MoveRow(long from, long to);
    }
}