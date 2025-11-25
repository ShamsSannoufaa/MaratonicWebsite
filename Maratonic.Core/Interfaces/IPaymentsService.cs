using Maratonic.Core.Entities;

namespace Maratonic.Core.Interfaces
{
    public interface IPaymentsService
    {
        Task<Payment> CreatePaymentAsync(int registrationId, decimal amount, string provider);
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task<IEnumerable<Payment>> GetPaymentsByUserAsync(string userId);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<bool> RefundPaymentAsync(int paymentId);
    }
}
