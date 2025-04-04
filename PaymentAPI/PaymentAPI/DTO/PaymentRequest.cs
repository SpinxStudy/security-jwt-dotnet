namespace PaymentAPI.DTO;

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string SourceAccountId { get; set; }
    public string DestinationAccountId { get; set; }
}
