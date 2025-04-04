using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransactionAPI.DTO;
using TransactionAPI.Services;

namespace TransactionAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "transaction_initiator")]
public class TransactionController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public TransactionController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTransaction([FromBody] TransactionRequest transactionRequest)
    {
        if (transactionRequest == null || transactionRequest.Amount <= 0 ||
            string.IsNullOrEmpty(transactionRequest.SourceAccountId) ||
            string.IsNullOrEmpty(transactionRequest.DestinationAccountId))
        {
            return BadRequest("Data transaction invalid!");
        }

        try
        {
            var result = await _paymentService.ProcessPaymentAsync(transactionRequest);
            return Ok(new
            {
                Message = "Transação criada com sucesso",
                PaymentDetails = result
            });

        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, $"Error during process payment: {ex.Message}");
        }
    }
}
