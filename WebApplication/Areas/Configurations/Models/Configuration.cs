using System.ComponentModel.DataAnnotations;

namespace CMS.Areas.Configurations.Models
{
    public class Configuration
    {
        [Key]
        [MaxLength(48, ErrorMessage = "Max 48")]
        public string Key { get; set; }
        public string NameKey { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }
    }
}
