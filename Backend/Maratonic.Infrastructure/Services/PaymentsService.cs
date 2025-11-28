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
        // 1) Mock Ödeme Oluşturma (Provider ile)
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

            // Registration payment status güncelle
            registration.PaymentStatus = mockResult.Status;

            _context.Payments.Add(payment);
            _context.Registrations.Update(registration);

            await _context.SaveChangesAsync();

            return payment;
        }

        // ================================
        // 2) Basit Ödeme Oluşturma (provider parametresiz)
        // ================================
        public async Task<Payment> CreatePaymentAsync(int registrationId, decimal amount)
        {
            var registration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.Id == registrationId);

            if (registration == null)
                throw new Exception("Registration not found");

            var payment = new Payment
            {
                RegistrationId = registrationId,
                Amount = amount,
                TransactionId = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                ProviderResponse = "",
                Status = PaymentStatus.Pending
            };

            registration.PaymentStatus = PaymentStatus.Pending;

            _context.Payments.Add(payment);
            _context.Registrations.Update(registration);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments
                .Include(p => p.Registration)
                .FirstOrDefaultAsync(p => p.Id == paymentId)
                ?? throw new Exception("Payment not found");
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserAsync(string userId)
        {
            return await _context.Payments
                .Include(p => p.Registration)
                .Where(p => p.Registration.UserId == userId)
                .ToListAsync();
        }

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
            var payment = await _context.Payments
                .Include(p => p.Registration)
                .FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
                throw new Exception("Payment not found");

            if (payment.Status == PaymentStatus.Refunded)
                throw new Exception("Payment already refunded");

            var mockRefund = _gateway.ProcessRefund(payment.TransactionId);

            payment.Status = PaymentStatus.Refunded;
            payment.RefundedAt = DateTime.UtcNow;
            payment.ProviderResponse = mockRefund.ProviderResponse;

            if (payment.Registration != null)
            {
                payment.Registration.PaymentStatus = PaymentStatus.Refunded;
                _context.Registrations.Update(payment.Registration);
            }

            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();

            return true;
        }

        // ================================
        // 6) ADMIN MANUAL APPROVE
        // ================================
        public async Task<Payment> ApprovePaymentForRegistrationAsync(int registrationId)
        {
            var registration = await _context.Registrations
                .Include(r => r.PaymentTransaction)
                .Include(r => r.Race)
                .FirstOrDefaultAsync(r => r.Id == registrationId);

            if (registration == null)
                throw new Exception("Registration not found");

            var amount = registration.Fee;

            Payment payment;

            if (registration.PaymentTransaction == null)
            {
                // Yeni payment yarat
                var mockResult = _gateway.ProcessPayment(amount, "AdminManual");

                payment = new Payment
                {
                    RegistrationId = registration.Id,
                    Amount = amount,
                    Provider = "AdminManual",
                    Status = mockResult.Status,
                    TransactionId = mockResult.TransactionId,
                    ProviderResponse = mockResult.ProviderResponse,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Payments.Add(payment);
            }
            else
            {
                // Var olanı Completed yap
                payment = registration.PaymentTransaction;
                payment.Status = PaymentStatus.Completed;
                _context.Payments.Update(payment);
            }

            registration.PaymentStatus = PaymentStatus.Completed;
            _context.Registrations.Update(registration);

            await _context.SaveChangesAsync();

            return payment;
        }
    }
}
