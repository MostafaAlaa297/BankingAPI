using BankingAPI.Data;
using BankingAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BankingAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankingContext _context;
        public string AccountType { get; protected set; }
        public string AccountNumber { get; protected set; }
        public AccountService(BankingContext context)
        {
            _context = context;
        }
        public virtual double Deposit(string accountNumber, string accountType, double amount)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber) ?? throw new Exception("Account not found");
            if (amount <= 0)
            {
                throw new InsufficientFundsException("Insufficient amount");
            }

            account.Balance += amount;
            account.AccountType = accountType;
            account.AccountNumber = accountNumber;

            var transaction = new Transaction
            {
                TransactionType = "Deposit",
                Amount = amount,
                Timestamp = DateTime.UtcNow,
                AccountNumber = accountNumber
            };

            _context.Transactions?.Add(transaction);
            _context.SaveChanges();

            return account.Balance;
        }

        public virtual double Withdraw(string accountNumber, string accountType, double amount)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber) ?? throw new Exception("Account not found");
            
            if (amount <= 0.0 & account.Balance <= 0.0)
            {
                throw new InsufficientFundsException("Insufficient amount");
            }

            account.AccountType = accountType;

            var transaction = new Transaction
            {
                TransactionType = "Withdraw",
                Amount = amount,
                Timestamp = DateTime.UtcNow,
                AccountNumber = accountNumber
            };

            _context.Transactions?.Add(transaction);
            _context.SaveChanges();

            return account.Balance;
        }
    }
}
