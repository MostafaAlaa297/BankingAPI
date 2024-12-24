using BankingAPI.Data;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.ComponentModel.DataAnnotations;

namespace BankingAPI.Models
{
    public class Account
    {
        [Key]
        public string AccountNumber { get; set; } 
        public double Balance { get; set; }
        public string AccountType { get; set; }

        public Account(string accountNumber, string accountType)
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            Balance = 0.0;
        }
    }
}
