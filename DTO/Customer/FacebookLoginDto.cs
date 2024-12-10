using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class FacebookLoginDto
    {
        public string email { get; set; } = string.Empty;
        public string id { get; set; }
        public string name { get; set; }
    }
}
