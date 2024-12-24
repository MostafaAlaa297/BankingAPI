using BankingAPI.Data;
using BankingAPI.Models;

namespace BankingAPI.Services
{
    public interface IAccountService
    {
        public double Deposit(string accountNumber, string accountType, double amount);
        public double Withdraw(string accountNumber, string accountType, double amount);
    }
}
