using System.Collections.Generic;

namespace DTO.Project
{
    public class ProjectCateDto
    {
        public long Pid { get; set; }
        public long ParentId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public List<ProjectCateDto> Children { get; set; }
    }
}
