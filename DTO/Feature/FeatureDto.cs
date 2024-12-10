using System.Collections.Generic;

namespace DTO.Feature
{
    public class FeatureDto
    {
        public long Pid { get; set; }
        public string Title { get; set; }
        public string TitleAlt { get; set; }
        public string TitleSEO { get; set; }
        public string Description { get; set; }
        public string DescriptionSEO { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string PicThumb { get; set; }
        public string PicFull { get; set; }
        public string ImageMeta { get; set; }
        public string PublishDate { get; set; }
        public string TagKey { get; set; }
        public string SlugTagKey { get; set; }
        public bool Enabled { get; set; }
        public Dictionary<string, string> ImageList { get; set; }
    }
}
