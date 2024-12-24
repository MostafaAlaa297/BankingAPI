using BankingAPI.Data;

namespace BankingAPI.Services
{
    public class BankSystem
    {
        private readonly BankingContext _context;
        private readonly AccountService _accountService;

        public BankSystem(BankingContext context, AccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }
        public bool TransferFunds(AccountService source, AccountService target, double amount)
        {
            var sourceAccount = _context.Accounts.FirstOrDefault(a => a.AccountNumber == source.AccountType);
            
            if (source == null || target == null)
            {
                throw new Exception("Source or Target cannot be null");
            }

            if (amount <= 0)
            {
                throw new InsufficientFundsException("Amount is cannot be zero or less");
            }
            source.Withdraw(_accountService.AccountNumber, source.AccountType, amount);
            target.Deposit(_accountService.AccountNumber, target.AccountType, amount);
            
            return true;
        }
    }
}
