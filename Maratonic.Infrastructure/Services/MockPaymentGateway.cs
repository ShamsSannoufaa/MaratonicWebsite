using Maratonic.Core.Enums;

namespace Maratonic.Infrastructure.Services
{
    public class MockPaymentGateway
    {
        // ödeme
        public MockPaymentResult ProcessPayment(decimal amount, string provider)
        {
            return new MockPaymentResult
            {
                TransactionId = Guid.NewGuid().ToString(),
                Status = PaymentStatus.Completed,
                ProviderResponse = $"Mock payment completed using provider: {provider}"
            };
        }

        // iade
        public MockRefundResult ProcessRefund(string transactionId)
        {
            return new MockRefundResult
            {
                ProviderResponse = $"Mock refund processed for transaction {transactionId}"
            };
        }
    }

    public class MockPaymentResult
    {
        public string TransactionId { get; set; }
        public PaymentStatus Status { get; set; }
        public string ProviderResponse { get; set; }
    }

    public class MockRefundResult
    {
        public string ProviderResponse { get; set; }
    }
}
