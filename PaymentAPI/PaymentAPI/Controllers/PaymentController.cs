using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.DTO;
using PaymentAPI.Enum;

namespace PaymentAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "payment_processor")]
public class PaymentController : ControllerBase
{
    [HttpPost("process")]
    public IActionResult ProcessPayment([FromBody] PaymentRequest paymentRequest)
    {
        if (paymentRequest == null || paymentRequest.Amount <= 0 ||
            string.IsNullOrEmpty(paymentRequest.SourceAccountId) ||
            string.IsNullOrEmpty(paymentRequest.DestinationAccountId))
        {
            return BadRequest("Payment data is invalid!");
        }

        // Simulate payment processing logic
        var response = new
        {
            TransactionId = Guid.NewGuid().ToString(),
            Status = StatusPayment.Processed,
            paymentRequest.Amount,
            paymentRequest.SourceAccountId,
            paymentRequest.DestinationAccountId,
            ProcessedAt = DateTime.UtcNow
        };

        return Ok(response);
    }
}
