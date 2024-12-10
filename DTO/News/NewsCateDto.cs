using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.News
{
    public class NewsCateDto
    {
        public long Pid { get; set; }
        public long ParentId { get; set; }
        public string Title { get; set; }
        public string TitleAlt { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public List<NewsCateDto> Children { get; set; }
    }
}
