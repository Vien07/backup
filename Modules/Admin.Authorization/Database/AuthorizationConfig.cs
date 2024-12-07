using Steam.Core.Base.Models;

namespace Admin.Authorization.Database
{
    public class AuthorizationConfig:BaseEntity
    {


        public string Type { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }



    }
}
