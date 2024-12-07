using Admin.Payments.Models;

namespace Admin.Payments.PayOS_Services
{
    public interface IPayOSService
    {
        Task<PaymentReturnDataModel> CreatePayOSPaymentLink(PaymentInformationModel model);
    }
}