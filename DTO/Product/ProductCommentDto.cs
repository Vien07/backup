using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product
{
    public class ProductCommentDto
    {
        public long ProductCommentPid { get; set; }
        public long CustomerPid { get; set; }
        public string FullName { get; set; }
        public string ReplyName { get; set; }
        public string CreateDate { get; set; }
        public int Like { get; set; }
        public string Comment { get; set; }
        public string PicThumb { get; set; }
        public int Star { get; set; }
        public List<ProductCommentDto> Children { get; set; }
    }
}
