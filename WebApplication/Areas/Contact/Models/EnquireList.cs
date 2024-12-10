using CMS.Areas.Feature.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Areas.Contact.Models
{
    public class EnquireList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public string FullNameEnquire { get; set; }
        public string PhoneEnquire { get; set; }
        public string EmailEnquire { get; set; }
        public string ContentEnquire { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public long ServiceDetailPid { get; set; }
        public FeatureDetail ServiceDetail { get; set; }
        public DateTime DateEnquire { get; set; }

        public DateTime ReadDate { get; set; }
        public DateTime RecivedDate { get; set; }
        public bool isRead { get; set; }
        public bool isDeleted { get; set; } = false;
        public bool isHidden { get; set; } = false;
    }
}
