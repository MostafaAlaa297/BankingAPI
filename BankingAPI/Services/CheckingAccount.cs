using BankingAPI.Data;

namespace BankingAPI.Services
{
    public class CheckingAccount : AccountService
    {
        private readonly BankingContext _context;
        public static double OverdraftLimit { get; protected set; }
        public CheckingAccount(double overdraftLimit, BankingContext context) : base(context)
        {
            OverdraftLimit = overdraftLimit;
            _context = context;
        }

        public override double Withdraw(string accountNumber, string accountType, double amount)
        {
            var account = _context.Accounts.FirstOrDefault(a =>  a.AccountNumber == accountNumber) ?? throw new Exception("Account not found");
            
            if (amount <= 0)
            {
                throw new InsufficientFundsException("Insufficient amount");
            }
            if (account.Balance - amount < -OverdraftLimit)
            {
                throw new InsufficientFundsException("Overdraft limit exceeded");
            }
            
            return base.Withdraw(accountNumber, "ChekingAccount", amount);
        }

    }
}
