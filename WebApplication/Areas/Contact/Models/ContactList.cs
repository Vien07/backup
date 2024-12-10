using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Contact.Models
{
    public class ContactList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }

        public DateTime ReadDate { get; set; }
        public DateTime RecivedDate { get; set; }
        public bool isRead { get; set; }
        public bool isDeleted { get; set; } = false;
        public bool isHidden { get; set; } = false;
    }
}
