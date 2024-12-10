using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Cart
{
    public class CartStringDto
    {
        public Guid id { get; set; }
        public int productId { get; set; }
        public int colorId { get; set; }
        public int optionId { get; set; }
        public int quantity { get; set; }
        public int productCateId { get; set; }
        public decimal price { get; set; }
        public decimal priceDiscount { get; set; }
    }
    public class CartItemDto
    {
        public Guid id { get; set; }
        public int quantity { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public string colorTitle { get; set; }
        public string colorCode { get; set; }
        public string picture { get; set; }
        public string slug { get; set; }
        public decimal price { get; set; }
        public string priceString { get; set; }
        public decimal priceOriginal { get; set; }
        public string priceOriginalString { get; set; }
        public decimal priceDiscount { get; set; }
        public string priceDiscountString { get; set; }
        public decimal total { get; set; }
        public string totalString { get; set; }
        public int optionId { get; set; }
        public string optionCode { get; set; }
        public string optionTitle { get; set; }
        public int colorId { get; set; }
        public int productId { get; set; }
        public long productCateId { get; set; }
        public int months { get; set; }
    }

    public class CartInfoDto
    {
        public List<CartItemDto> items { get; set; }
        public int totalQuantity { get; set; }
        public decimal totalPrice { get; set; }
        public string totalPriceString { get; set; }
        public decimal temporaryPrice { get; set; }
        public string temporaryPriceString { get; set; }
    }

    public class OrderInformation
    {
        public long Pid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TaxCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Address { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Note { get; set; }
        public long ProductDetailPid { get; set; }
        public long ProductCatePid { get; set; }
        public bool VAT { get; set; }
        public bool IsPayment { get; set; }
        public int OrderState { get; set; }
        public int PaymentMethod { get; set; }
        public decimal ShipFee { get; set; }
        public decimal Deposit { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderString { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public IFormFile Background { get; set; }
        public string DiscountCodeValue { get; set; }
        public string DiscountCodeType { get; set; }
        public string DiscountCode { get; set; }
    }

    public class CardInformation
    {
        public long Pid { get; set; }
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
        public long OrderPid { get; set; }
        public IFormFile Avatar { get; set; }
        public List<CustomLink> CustomLink { get; set; }
        public string PersonalLink { get; set; }
        public string CompanyWebsiteLink { get; set; }
        public string PersonalWebsiteLink { get; set; }
    }
    public class CustomLink
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
