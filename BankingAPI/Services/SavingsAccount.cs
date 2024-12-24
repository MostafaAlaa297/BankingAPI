using BankingAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Services
{
    public class SavingsAccount: AccountService
    {
        public static float InterestRate { get; protected set; }
        public static string accountType { get; private set; }

        private readonly BankingContext _context;
        public SavingsAccount(float interestRate, BankingContext context) : base (context)
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
                throw new InsufficientFundsException("Balance exceeding limit");
            }
            return base.Withdraw(accountNumber, "SavingsAccount", amount);
        }

        public void ApplyMonthlyInterest(string accountNumber)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber) ?? throw new Exception("Account not found");

            account.Balance += account.Balance * InterestRate;
        }

    }
}
