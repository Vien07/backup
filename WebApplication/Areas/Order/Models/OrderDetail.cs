using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Areas.Order.Models
{
    public class OrderDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Pid { get; set; }
        public long OrderPid { get; set; }
        public virtual Order Order { get; set; }

        public Guid UID { get; set; }
        public long ProductDetailPid { get; set; }
        public long OptionId { get; set; }
        public string OptionName { get; set; }
        public long ColorId { get; set; }
        public string ColorName { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }

         
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string CreateUser { get; set; }
        public System.Nullable<DateTime> CreateDate { get; set; } = DateTime.Now;
        [MaxLength(24, ErrorMessage = "Max 24")]
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public DateTime ExpiredDate { get; set; } = DateTime.Now;

        public string FullName { get; set; }
        public string PersonalPhone { get; set; }
        public string HomePhone { get; set; }
        public string CompanyPhone { get; set; }
        public string PersonalEmail { get; set; }
        public string WorkEmail { get; set; }
        public string Position { get; set; }
        public string Url { get; set; }
        public string Telegram { get; set; }
        public string WhatsApp { get; set; }
        public string Viber { get; set; }
        public string Skype { get; set; }
        public string Zalo { get; set; }
        public string LinkedIn { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string CustomLink { get; set; }
        public string Avatar { get; set; }
        public string QrImage { get; set; }
        public string QrLink { get; set; }
        public string PersonalLink { get; set; }
        public string CompanyWebsiteLink { get; set; }
        public string PersonalWebsiteLink { get; set; }

    }
}
