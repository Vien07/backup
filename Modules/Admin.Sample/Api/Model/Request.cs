
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Sample.Api.Models.Request
{
    public class SampleModel
    {
        public long Id { get; set; }

        public string PostSlug { get; set; } = String.Empty;
    }
    public class LoginModel
    {
        public long Id { get; set; }

        public string Password { get; set; }
        public string Username { get; set; }
    }



}
