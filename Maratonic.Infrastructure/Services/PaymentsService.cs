using Maratonic.Core.Entities;
using Maratonic.Core.Interfaces;

namespace Maratonic.Infrastructure.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly AppDbContext _context;

        public PaymentsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreatePaymentAsync(int registrationId, decimal amount)
        {
            var payment = new Payment
            {
                RegistrationId = registrationId,
                Amount = amount,
                TransactionId = Guid.NewGuid().ToString(),
                PaymentDate = DateTime.UtcNow,
                IsSuccessful = false,
                ProviderResponse = ""
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }
    }
}
