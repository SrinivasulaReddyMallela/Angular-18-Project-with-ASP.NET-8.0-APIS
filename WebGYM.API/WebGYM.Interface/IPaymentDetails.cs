using System.Linq;
using WebGYM.Models;
using WebGYM.ViewModels;

namespace WebGYM.Interface
{
    public interface IPaymentDetails
    {
        Task<IQueryable<PaymentDetailsViewModel>> GetAll(QueryParameters queryParameters, int userId);
        Task<int> Count(int userId);
        Task<bool> RenewalPayment(RenewalViewModel renewalViewModel);
    }
}