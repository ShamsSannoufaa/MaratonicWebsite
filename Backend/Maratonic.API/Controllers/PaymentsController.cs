using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Maratonic.Core.Interfaces;
using Maratonic.API.DTOs;

namespace Maratonic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsService _paymentsService;

        public PaymentsController(IPaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        // ============================================================
        // 1) CREATE MOCK PAYMENT
        // ============================================================
        [HttpPost("create")]
        [Authorize(Roles = "ATHLETE,ADMIN")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestDto request)
        {
            var payment = await _paymentsService.CreatePaymentAsync(
                request.RegistrationId,
                request.Amount,
                request.Provider
            );

            return Ok(new
            {
                message = "Payment created successfully.",
                paymentId = payment.Id,
                registrationId = payment.RegistrationId,
                amount = payment.Amount,
                status = payment.Status.ToString(),
                transactionId = payment.TransactionId,
                createdAt = payment.CreatedAt
            });
        }

        // ============================================================
        // 2) GET PAYMENT BY ID
        // ============================================================
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPayment(int paymentId)
        {
            var payment = await _paymentsService.GetPaymentByIdAsync(paymentId);
            return Ok(payment);
        }

        // ============================================================
        // 3) GET PAYMENTS BY USER (ADMIN)
        // ============================================================
        [HttpGet("user/{userId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetPaymentsByUser(string userId)
        {
            var list = await _paymentsService.GetPaymentsByUserAsync(userId);
            return Ok(list);
        }

        // ============================================================
        // 4) GET ALL PAYMENTS (ADMIN)
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _paymentsService.GetAllPaymentsAsync();
            return Ok(list);
        }

        // ============================================================
        // 5) REFUND PAYMENT
        // ============================================================
        [HttpPost("refund/{paymentId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RefundPayment(int paymentId)
        {
            var success = await _paymentsService.RefundPaymentAsync(paymentId);

            if (!success)
                return BadRequest("Refund failed.");

            return Ok(new
            {
                message = "Payment refunded successfully.",
                refundedPaymentId = paymentId
            });
        }
    }
}
