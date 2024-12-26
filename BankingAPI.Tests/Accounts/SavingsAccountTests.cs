using BankingAPI.Controllers;
using BankingAPI.Data;
using BankingAPI.Migrations;
using BankingAPI.Models;
using BankingAPI.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankingAPI.Data.BankingContext;

namespace BankingAPI.Tests.Accounts
{
    public class SavingsAccountTests
    {
        private readonly BankingContext _bankingContext; 
        public SavingsAccountTests() {
            _bankingContext = BankingContextFactory.CreateContext();
            SeedData();
        }
        private void SeedData()
        {
            _bankingContext.Accounts.RemoveRange(_bankingContext.Accounts);
            _bankingContext.SaveChanges();
            _bankingContext.Accounts.Add(new Account
            {
                AccountNumber = "S0012345678901234",
                AccountType = "Savings",
                Balance = 1000.0,
                InterestRate = 5.0f 
            });
            _bankingContext.SaveChanges();
        }
        public void Dispose()
        {
            _bankingContext.Dispose();
        }

        [Fact]
        public void CalculateInterest_ShouldReturnCorrectInterest()
        {
            var interestRate = 5.0f;
            var savingsAccount = new SavingsAccountService(interestRate, _bankingContext);
            var balance = 1000.0;
            var months = 6;

            var interest = savingsAccount.CalculateInterest("S0012345678901234", months);
            Assert.Equal(300.0, interest);
        }

        [Fact]
        public void CalculateInterest_WithZeroMonths_ShouldReturnZeroInterest()
        {
            var interestRate = 5.0f;
            var savingsAccount = new SavingsAccountService(interestRate, _bankingContext);
            var balance = 1000.0;
            var months = 0;

            var interest = savingsAccount.CalculateInterest("S0012345678901234", months);
            Assert.Equal(0.0, interest);
        }

        [Fact]
        public void CalculateInterest_WithNegativeMonths_ShouldReturnNegativeInterest()
        {
            var interestRate = 5.0f;
            var savingsAccount = new SavingsAccountService(interestRate, _bankingContext);
            var balance = 1000.0;
            var months = -6;

            var interest = savingsAccount.CalculateInterest("S0012345678901234", months);
            Assert.Equal(-300.0, interest);
        }
    }
}
