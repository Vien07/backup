using System.Collections.Generic;
using System.Threading.Tasks;
using CMS.Areas.Contact.Models;
using DTO.Cart;
using DTO.Customer;

namespace CMS.Repository
{
    public interface ICart_Repository
    {
        Task<string> GetCartInfo(string cartStr, string lang);
        Task<dynamic> SaveOrder(string cartStr, string lang, OrderInformation information);
        Task<OrderDto> GetOrder(string Pid);
    }
}