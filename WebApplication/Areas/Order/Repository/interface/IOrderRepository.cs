using DTO.Cart;
using DTO.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CMS.Areas.Order
{
    public interface IOrderRepository
    {
        dynamic LoadData(SearchDto search);
        dynamic Edit(int Pid);
        dynamic EditCard(int Pid);
        dynamic Insert(OrderInformation order);
        dynamic Update(OrderInformation order);
        bool Enable(long[] Pid, bool active);
        bool ChangeStatus(long[] Pid, int active);
        bool ChangePaymentMethod(long[] Pid, int active);
        bool ChangeIsPayment(long[] Pid, bool active);
        Task<bool> SendMail(long[] Pid);
        Task<bool> ExportVAT(long[] Pid, bool isSendMail);
        bool Delete(int Pid);
        dynamic Delete(long[] Pid);
        string LoadProductList(string key, int catePid);
        string LoadProductDetail(long productId);
        string LoadOrderTable(string orderString, decimal shipFee, decimal deposit);
        dynamic GetInfoCustomer(string email);
        decimal GetPrice(long ProductPid, long ProductCatePid);
        dynamic InsertCard(CardInformation card);
        dynamic UpdateCard(CardInformation card);
        dynamic LoadDataCard(SearchDto search);

        bool DeleteCard(int Pid);
        dynamic DeleteCard(long[] Pid);
        dynamic UploadCSV(IFormFile File, string OrderPid);
        dynamic ExportCSV(string OrderPid);
        dynamic ApplyDiscountCode(decimal price, string discountCode);
    }
}