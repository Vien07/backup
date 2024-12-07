using Admin.Payments.Models;

namespace Admin.Payments.Services
{
    public interface IPaymentService
    {
        IList<string> GetActivePaymentMethods();

        Task<PaymentReturnDataModel> ProcessPayment(PaymentInformationModel paymentInfo);
    }
}