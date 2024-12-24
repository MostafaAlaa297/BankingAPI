namespace BankingAPI.Controllers
{
    public class WithdraRequest
    {
        public string? AccountNumber { get; set; }
        public string AccountType { get; set; }
        public double Amount { get; set; }
    }
}
