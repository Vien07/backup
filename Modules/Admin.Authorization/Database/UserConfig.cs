
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Authorization.Database
{
    public class UserConfig
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }

        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public System.Nullable<DateTime> UpdateDate { get; set; }

    }
}
