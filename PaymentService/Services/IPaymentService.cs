using PaymentService.Models;
using PaymentService.ViewModels;

namespace PaymentService.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentViewModel>> GetAsync();
        Task<PaymentViewModel> GetByIdAsync(int id);
        Task<PaymentViewModel> CreateAsync(PaymentCreateViewModel payment);
        Task<PaymentViewModel> UpdateAsync(int id, PaymentStatus status);
    }
}
