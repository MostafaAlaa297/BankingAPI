namespace BankingAPI.Controllers
{
    public class CreateAccountRequest
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public float? InterestRate { get; set; }
        public double? OverdraftLimit { get; set; }
    }
}
