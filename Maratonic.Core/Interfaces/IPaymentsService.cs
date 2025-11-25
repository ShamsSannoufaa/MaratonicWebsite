using Maratonic.Core.Entities;

namespace Maratonic.Core.Interfaces
{
    public interface IPaymentsService
    {
        Task<Payment> CreatePaymentAsync(int registrationId, decimal amount);
    }
}
