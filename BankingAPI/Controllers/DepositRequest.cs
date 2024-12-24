namespace BankingAPI.Controllers
{
    public class DepositRequest
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public double Amount { get; set; }
    }
}