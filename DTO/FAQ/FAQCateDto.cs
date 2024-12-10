﻿using System.Collections.Generic;

namespace DTO.FAQ
{
    public class FAQCateDto
    {
        public long Pid { get; set; }
        public long ParentId { get; set; }
        public string Title { get; set; }
        public string TitleAlt { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public List<FAQCateDto> Children { get; set; }
    }
}
