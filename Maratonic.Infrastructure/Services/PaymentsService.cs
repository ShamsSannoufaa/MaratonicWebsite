using Maratonic.Core.Entities;
using Maratonic.Core.Enums;
using Maratonic.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Maratonic.Infrastructure.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly AppDbContext _context;
        private readonly MockPaymentGateway _gateway;

        public PaymentsService(AppDbContext context)
        {
            _context = context;
            _gateway = new MockPaymentGateway();
        }

        // ================================
        // 1) Mock Ödeme Oluşturma
        // ================================
        public async Task<Payment> CreatePaymentAsync(int registrationId, decimal amount, string provider)
        {
            var registration = await _context.Registrations
                .Include(r => r.PaymentTransaction)
                .FirstOrDefaultAsync(r => r.Id == registrationId);

            if (registration == null)
                throw new Exception("Registration not found");

            if (registration.PaymentTransaction != null)
                throw new Exception("Payment already exists for this registration");

            // MOCK ödeme yap
            var mockResult = _gateway.ProcessPayment(amount, provider);

            var payment = new Payment
            {
                RegistrationId = registration.Id,
                Amount = amount,
                Provider = provider,
                Status = mockResult.Status,
                TransactionId = mockResult.TransactionId,
                ProviderResponse = mockResult.ProviderResponse,
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        // ================================
        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Registration)
                .FirstOrDefaultAsync(p => p.Id == paymentId)
                ?? throw new Exception("Payment not found");
        }

        // ================================
        public async Task<IEnumerable<Payment>> GetPaymentsByUserAsync(string userId)
        {
            return await _context.Payments
                .Include(p => p.Registration)
                .Where(p => p.Registration.UserId == userId)
                .ToListAsync();
        }

        // ================================
        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Registration)
                .ToListAsync();
        }

        // ================================
        // 5) REFUND
        // ================================
        public async Task<bool> RefundPaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
                throw new Exception("Payment not found");

            if (payment.Status == PaymentStatus.Refunded)
                throw new Exception("Payment already refunded");

            var mockRefund = _gateway.ProcessRefund(payment.TransactionId);

            payment.Status = PaymentStatus.Refunded;
            payment.RefundedAt = DateTime.UtcNow;
            payment.ProviderResponse = mockRefund.ProviderResponse;

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
