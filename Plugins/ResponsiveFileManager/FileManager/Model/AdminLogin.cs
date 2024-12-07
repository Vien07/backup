using Microsoft.AspNetCore.Http;
using System.IO;

namespace Steam.Core.FileManager.Model
{
    public class AdminLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AccessKey { get; set; }
    }

}