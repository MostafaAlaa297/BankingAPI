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
        public double? Amount { get; protected set; }

        public AccountService(string accountnumber, string accountType, double amount, BankingContext context)
        {
            AccountNumber = accountnumber;
            AccountType = accountType;
            Amount = amount;
            _context = context;
        }
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

            var transaction = new Transaction(accountNumber, "Deposit", amount, DateTime.UtcNow);

            _context.Transactions?.Add(transaction);
            _context.SaveChanges();

            Console.WriteLine($"Your current balance is: {account.Balance}");
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

            var transaction = new Transaction(accountNumber, "Withdraw", amount, DateTime.UtcNow);
            
            account.Balance -= amount;

            _context.Transactions?.Add(transaction);
            _context.SaveChanges();
            Console.WriteLine($"Your current balance is: {account.Balance}");
            return account.Balance;
        }
    }
}
