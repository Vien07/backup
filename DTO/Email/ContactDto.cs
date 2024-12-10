using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmsModels.EmailModels
{
    public class ContactDto
    {
        public string FullName { get; set; }
        public string Subject { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
    }
}
