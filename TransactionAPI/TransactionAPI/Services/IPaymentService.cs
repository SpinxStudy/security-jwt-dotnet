using TransactionAPI.DTO;

namespace TransactionAPI.Services;

public interface IPaymentService
{
    Task<object> ProcessPaymentAsync(TransactionRequest request);
}