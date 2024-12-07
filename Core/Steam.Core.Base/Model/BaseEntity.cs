using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Steam.Core.Base.Models
{

    public class BaseEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public double? Order { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
      
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class IsMultilangAttribute : Attribute
    {
        public bool IsMultilang { get; }

        public IsMultilangAttribute(bool isMultilang)
        {
            IsMultilang = isMultilang;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CustomTypeAttribute : Attribute
    {
        public class TypeData
        {
            public const string MULTILANG = "MULTILANG";
        }
        public string Type { get; }

        public CustomTypeAttribute(string type)
        {
            Type = type;
        }
    }
}