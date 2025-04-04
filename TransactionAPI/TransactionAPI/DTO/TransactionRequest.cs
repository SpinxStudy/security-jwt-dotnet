namespace TransactionAPI.DTO;

public class TransactionRequest
{
    public decimal Amount { get; set; }
    public string SourceAccountId { get; set; }
    public string DestinationAccountId { get; set; }
}
