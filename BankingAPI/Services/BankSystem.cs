using BankingAPI.Data;
using BankingAPI.Models;
using System.Diagnostics;

namespace BankingAPI.Services
{
    public class BankSystem
    {
        private readonly BankingContext _context;
        
        public BankSystem() { }
        public BankSystem(BankingContext context)
        {
            _context = context;
        }
        public double[] TransferFunds(string sourceAccountNumber, string targetAccountNumber, double amount)
        {
            var sourceAccount = _context.Accounts.FirstOrDefault(a => a.AccountNumber == sourceAccountNumber);
            var targetAccount = _context.Accounts.FirstOrDefault(a => a.AccountNumber == targetAccountNumber);
            if (sourceAccountNumber == null || targetAccountNumber == null || sourceAccount == null || targetAccount == null) throw new KeyNotFoundException("Account not found");

            if (sourceAccountNumber == null || targetAccountNumber == null)
            {
                throw new Exception("Source or Target cannot be null");
            }

            if (amount <= 0)
            {
                throw new InsufficientFundsException("Amount is cannot be zero or less");
            }

            double SourceBalance;
            double TargetBalance;
            if (sourceAccount.AccountType == "Savings")
            {
                var savingsAccount = new SavingsAccountService(0.05f, _context);
                SourceBalance = savingsAccount.Withdraw(sourceAccount.AccountNumber, sourceAccount.AccountType, amount);
            }
            else if (sourceAccount.AccountType == "Checking")
            {
                var checkingAccount = new CheckingAccountService(500, _context);
                SourceBalance = checkingAccount.Withdraw(sourceAccount.AccountNumber, sourceAccount.AccountType, amount);
            }
            else
            {
                throw new Exception("Invalid account type");
            }

            var accountService = new AccountService(targetAccount.AccountNumber, targetAccount.AccountType, amount, _context);
            TargetBalance = accountService.Deposit(targetAccount.AccountNumber, targetAccount.AccountType, amount);

            var transferTransaction = new Transaction(sourceAccount.AccountNumber, "Transfer", amount, DateTime.UtcNow);
            _context.Transactions.Add(transferTransaction);

            var withdrawTransaction = new Transaction(sourceAccount.AccountNumber, "Withdraw", amount, DateTime.UtcNow);
            _context.Transactions.Add(withdrawTransaction);

            var depositTransaction = new Transaction(targetAccount.AccountNumber, "Deposit", amount, DateTime.UtcNow);
            _context.Transactions.Add(depositTransaction);

            _context.SaveChanges();

            double[] resultArray = { sourceAccount.Balance, targetAccount.Balance };

            return resultArray;
        }
    }
}
