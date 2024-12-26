using BankingAPI.Data;

namespace BankingAPI.Services
{
    public class CheckingAccountService : AccountService
    {
        private readonly BankingContext _context;
        public static double OverdraftLimit { get; protected set; }
        private static double OverdraftFee = 20.0;
        public CheckingAccountService(double overdraftLimit, BankingContext context) : base(context)
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
            if (account.Balance - amount < 0)
            {
                account.Balance -= OverdraftFee;
            }
            
            return base.Withdraw(accountNumber, "Checking", amount);
        }
    }
}
