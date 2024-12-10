using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace DTO.Cart
{
    public class OrderListDto
    {
        public long Pid { get; set; }
        public string OrderDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string IsPayment { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Total { get; set; }
        public string TotalString { get; set; }
        public string Code { get; set; }
        public string ShipFeeString { get; set; }
        public string DepositString { get; set; }
        public string Status { get; set; }
        public string CompanyName { get; set; }
        public string TaxCode { get; set; }
        public string CompanyAddress { get; set; }
        public int UserLimit { get; set; }
        public int CurrentUserAmount { get; set; }
        public string RegisterDateString { get; set; }
        public string ExpiredDateString { get; set; }
        public int Months { get; set; }
        public long ProductId { get; set; }
        public long ProductCateId { get; set; }
        public string ProductTitle { get; set; }
        public string BackgroundImage { get; set; }
    }
    public class NameCard
    {
        public long Pid { get; set; }
        public long OrderPid { get; set; }
        public string FullName { get; set; }
        public string PersonalPhone { get; set; }
        public string CompanyPhone { get; set; }
        public string HomePhone { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
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
        public List<CustomLink> CustomLink { get; set; }
        public string Avatar { get; set; }
        public string Background { get; set; }
        public string QrImage { get; set; }
        public string QrLink { get; set; }
        public string PersonalLink { get; set; }
        public string CompanyWebsiteLink { get; set; }
        public string PersonalWebsiteLink { get; set; }

    }
    public class NameCardPaginationModel
    {
        public IPagedList PagedList { get; set; }

    }
    public class OrderImage
    {
        public long Pid { get; set; }
        public bool Active { get; set; }
        public string BackgroundImageUrl { get; set; }
    }
}
