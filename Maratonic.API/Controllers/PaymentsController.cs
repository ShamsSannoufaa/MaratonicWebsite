using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Maratonic.Core.Interfaces;
using Maratonic.API.DTOs;

namespace Maratonic.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsService _paymentsService;

        public PaymentsController(IPaymentsService paymentsService)
        {
            _paymentsService = paymentsService;
        }

        // =============================
        // Create Payment
        // =============================
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestDto request)
        {
            var payment = await _paymentsService.CreatePaymentAsync(
                request.RegistrationId,
                request.Amount,
                request.Provider
            );

            return Ok(payment);
        }

        // =============================
        [HttpGet("{paymentId}")]
        [Authorize]
        public async Task<IActionResult> GetPayment(int paymentId)
        {
            var payment = await _paymentsService.GetPaymentByIdAsync(paymentId);
            return Ok(payment);
        }

        // =============================
        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentsByUser(string userId)
        {
            var list = await _paymentsService.GetPaymentsByUserAsync(userId);
            return Ok(list);
        }

        // =============================
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _paymentsService.GetAllPaymentsAsync();
            return Ok(list);
        }

        // =============================
        // REFUND
        // =============================
        [HttpPost("refund/{paymentId}")]
        [Authorize]
        public async Task<IActionResult> RefundPayment(int paymentId)
        {
            var success = await _paymentsService.RefundPaymentAsync(paymentId);

            if (!success)
                return BadRequest("Refund failed.");

            return Ok("Payment refunded successfully.");
        }
    }
}
