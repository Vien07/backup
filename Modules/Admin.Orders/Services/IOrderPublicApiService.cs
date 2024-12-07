using Admin.Orders.Models.Dto;
using Steam.Core.Infrastructure.HttpHandle;

namespace Admin.Orders.Services
{
    public interface IOrderPublicApiService
    {
        Task<IApplicationResponse> Checkout(CheckoutModel model);
    }
}