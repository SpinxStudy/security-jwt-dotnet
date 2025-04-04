using TransactionAPI.DTO;
using TransactionAPI.Helper;
using TransactionAPI.Services;

namespace TransactionAPI.Services;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;

    public PaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:5000");
    }

    public async Task<object> ProcessPaymentAsync(TransactionRequest transactionRequest)
    {
        var token = Authentication.GenerateJwtToken();

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var paymentRequest = new
        {
            Amount = transactionRequest.Amount,
            SourceAccountId = transactionRequest.SourceAccountId,
            DestinationAccountId = transactionRequest.DestinationAccountId
        };

        var response = await _httpClient.PostAsJsonAsync("/api/payment/process", paymentRequest);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<object>();
    }
}
