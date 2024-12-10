using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class CustomerCookieDto
    {
        public string email { get; set; }
        public long Pid { get; set; }
        public string password { get; set; } = string.Empty;
        public bool IsLoginGoogle { get; set; } = false;
        public string PicThumb { get; set; } = string.Empty;
        public bool IsLoginFacebook { get; set; } = false;
        public string GoogleId { get; set; } = "";
        public string FacebookId { get; set; } = "";
    }
}
