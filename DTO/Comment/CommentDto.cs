using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Comment
{
    public class CommentDto
    {
        public string Title { get; set; }
        public string TitleAlt { get; set; }
        public string Description { get; set; }
        public string PicThumb { get; set; }
        public string Image { get; set; }
        public int Star { get; set; }
    }
}
