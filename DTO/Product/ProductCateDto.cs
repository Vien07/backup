using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product
{
    public class ProductCateDto
    {
        public long Pid { get; set; }
        public long ParentId { get; set; }
        public string Title { get; set; }
        public string TitleAlt { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string PicThumb { get; set; }
        public List<ProductCateDto> Children { get; set; }
    }
}
