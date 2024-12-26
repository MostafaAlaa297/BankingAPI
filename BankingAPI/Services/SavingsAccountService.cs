using BankingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Services
{
    public class SavingsAccountService: AccountService
    {
        public static float InterestRate { get; private set; }
        public static string accountType { get; private set; }

        private readonly BankingContext _context;
        public SavingsAccountService(float interestRate, BankingContext context) : base (context)
        {
            InterestRate = interestRate;
            _context = context;
        }
        public override double Withdraw(string accountNumber, string accountType, double amount)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber) ?? throw new Exception("Account not found");

            if (amount <= 0)
            {
                throw new InsufficientFundsException("Insufficient amount");
            }
            if (amount > account.Balance)
            {
                throw new InsufficientFundsException("Amount exceeding available balance");
            }
            return base.Withdraw(accountNumber, "Savings", amount);
        }

        public int CalculateInterest(string accountNumber, int months)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber) ?? throw new Exception("Account not found");
            return (int)(account.Balance * (InterestRate / 100) * months); 
        }
    }
}
