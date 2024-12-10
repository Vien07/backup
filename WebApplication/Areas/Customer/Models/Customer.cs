using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Customer.Models
{
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pid { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PicThumb { get; set; } = string.Empty;

        public string GoogleId { get; set; } = string.Empty;
        public string FacebookId { get; set; } = string.Empty;

        public string Email { get; set; }
        public string Password { get; set; }
        [MaxLength(48, ErrorMessage = "Max 48")]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(6, ErrorMessage = "Max 6")]
        public string ActivationCode { get; set; }
        public DateTime? CodeExpirationDate { get; set; }

        public string Note { get; set; }

        [MaxLength(2, ErrorMessage = "Max 2")]
        public string DateOfBirth { get; set; } = string.Empty;
        [MaxLength(2, ErrorMessage = "Max 2")]
        public string MonthOfBirth { get; set; } = string.Empty;
        [MaxLength(4, ErrorMessage = "Max 4")]
        public string YearOfBirth { get; set; } = string.Empty;

        public string Address { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }

        public string CompanyName { get; set; }
        public string TaxCode { get; set; }
        public string CompanyAddress { get; set; }

        public string Facebook { get; set; } = string.Empty;
        public string LinkedIn { get; set; } = string.Empty;
        public string Twitter { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Zalo { get; set; } = string.Empty;

        public bool Deleted { get; set; } = false;
        public bool Enabled { get; set; } = false;


        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public System.Nullable<DateTime> LastLogin { get; set; } = DateTime.Now;
    }
}
