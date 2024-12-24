namespace BankingAPI.Controllers
{
    public class TransferRequest
    {
        public string SourceAccountNumber { get; set; }
        public string TargetAccountNumber { get; set; }
        public double Amount { get; set; }
    }
}
