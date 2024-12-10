using DTO.Cart;
using DTO.Common;
using DTO.Customer;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace CMS.Repository
{
    public interface ICustomer_Repository
    {

        CustomerDto GetProfile();
        Task<List<OrderListDto>> GetOrderList();
        Task<ResponseDto> UpdateProfile(CustomerUpdateDto model, IFormFile avatar);
        Task<ResponseDto> UpdatePassword(string currentPassword, string newPassword);
        Task<ResponseDto> Register(CustomerDto model);
        Task<ResponseDto> Login(string email, string password, bool rememberMe);
        Task<bool> Active(string email, string code);
        Task<bool> ChangePassword(string email, string code);
        Task<ResponseDto> EditPassword(string code, string email, string password);
        Task<bool> LoginGoogle(string verifyString);
        Task<bool> LoginFB(string token);
        ResponseDto SendMailForgotPassword(string email, string newPassword);
        Task<string> ChangePasswordForEmail(string email);
        Task<OrderListDto> GetOrderByPid(string pid);
        Task<List<OrderImage>> GetOrderImages(string pid);
        Task<NameCard> GetNameCard(string url);
        Task<(string, string)> RenderHtmlForTable(IPagedList<NameCard> data);
        Task<IPagedList<NameCard>> GetListNameCardByOrderId(string orderId, int pageIndex, int pageSize, string searchKeyword = "");
        Task<dynamic> SearchListCard(SearchNameCardDto model);
        dynamic UploadBackgroundImage(IFormFile background, string orderId);
        dynamic UpdateBackgroundImageFromList(string imageId, string orderId);
        dynamic UploadCSV(IFormFile File, string OrderPid);
        dynamic ExportCSV(string OrderPid);
        dynamic EditCard(int Pid);
        dynamic InsertCard(CardInformation card);
        dynamic UpdateCard(CardInformation card);
        dynamic DeleteCard(int Pid);
        NameCard GetNameCardById(string cardId);
    }
}