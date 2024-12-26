using BankingAPI.Data;
using BankingAPI.Models;
using BankingAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BankingAPI.Data.BankingContext;

namespace BankingAPI.Tests.Accounts
{
    public class CheckingAccountTests
    {
        private readonly BankingContext _bankingContext;
        public CheckingAccountTests()
        {
            _bankingContext = BankingContextFactory.CreateContext();
            SeedData();
        }

        private void SeedData()
        {
            _bankingContext.Accounts.RemoveRange(_bankingContext.Accounts);
            _bankingContext.SaveChanges();
            _bankingContext.Accounts.Add(new Account
            {
                AccountNumber = "C0012345678901234",
                AccountType = "Checking",
                Balance = 1000.0,
                OverdraftLimit = 500
            });
            _bankingContext.SaveChanges();
        }

        public void Dispose()
        {
            _bankingContext.Dispose();
        }

        [Fact]
        public void Withdraw_WithSufficientBalance_ShouldSucced()
        {
            var checkingAccount = new CheckingAccountService(500, _bankingContext);
            var remainingBalance = checkingAccount.Withdraw("C0012345678901234", "Checking", 500.0);
            Assert.Equal(500.0, remainingBalance);
        }
        [Fact]
        public void Withdraw_WithOverdraft_ShouldApplyFee()
        {
            var checkingAccount = new CheckingAccountService(500, _bankingContext);
            var remainingBalance = checkingAccount.Withdraw("C0012345678901234", "Checking", 1200.0);
            Assert.Equal(-220.0, remainingBalance);
        }
        [Fact]
        public void Withdraw_ExceedingOverdraftLimit_ShouldThowException()
        {
            var checkingAccount = new CheckingAccountService(500, _bankingContext);
            Assert.Throws<InsufficientFundsException>(() => checkingAccount.Withdraw("C0012345678901234", "Checking", 2000.0));
        }
    }
}
