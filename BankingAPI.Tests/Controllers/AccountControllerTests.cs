using BankingAPI.Controllers;
using BankingAPI.Data;
using BankingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using static BankingAPI.Data.BankingContext;

namespace BankingAPI.Tests.Controllers
{
    public class AccountControllerTests
    {
        private readonly AccountController _accountController;
        private readonly BankingContext _bankingContext;

        public AccountControllerTests()
        {
            _bankingContext = BankingContextFactory.CreateContext();
            _accountController = new AccountController(_bankingContext);

            SeedData();
        }

        private void SeedData()
        {
            _bankingContext.Accounts.RemoveRange(_bankingContext.Accounts);
            _bankingContext.SaveChanges();

            _bankingContext.Accounts.Add(new Account { AccountNumber = "0123456789012345", AccountType = "Savings", Balance = 1000.0 });
            _bankingContext.Accounts.Add(new Account { AccountNumber = "1111456789012345", AccountType = "Checking", Balance = 1000.0 });
            _bankingContext.SaveChanges();
        }

        public void Dispose()
        {
            _bankingContext.Dispose();
        }

        [Fact]
        public async Task CreateAccount_ShouldCreateNewAccount()
        {
            var request = new CreateAccountRequest
            {
                AccountNumber = "1234567890123456",
                AccountType = "Checking"
            };
            var result = await _accountController.Create(request) as OkObjectResult;

            var response = result.Value as dynamic;
            Assert.Equal("1234567890123456", (string)response.AccountNumber);
            Assert.Equal("Checking", (string)response.AccountType);
            Assert.Equal(0.0, (double)response.Balance);
        }

        [Fact]
        public async Task Deposit_ShouldIncreaseBalance()
        {
            var request = new DepositRequest
            {
                AccountNumber = "0123456789012345",
                AccountType = "Savings",
                Amount = 50.0
            };

            var result = await _accountController.Deposit(request) as OkObjectResult;

            Assert.NotNull(result);
            var response = result.Value as double?;

            Assert.NotNull(response);
            Assert.Equal(1050.0, response);
        }


        [Fact]
        public async Task Withdraw_ShouldDecreseBalance()
        {
            var request = new WithdraRequest
            {
                AccountNumber = "0123456789012345",
                AccountType = "Savings",
                Amount = 50.0
            };

            var result = await _accountController.Withdraw(request) as OkObjectResult;

            var response = result.Value as double?;
            Assert.Equal(950.0, response);
        }

        [Fact]
        public async Task Transfer_ShouldUpdateBothAccounts()
        {
            var sourceAccountNumber = "1111456789012345";
            var targetAccountNumber = "0123456789012345";
            var request = new TransferRequest
            {
                SourceAccountNumber = sourceAccountNumber,
                TargetAccountNumber = targetAccountNumber,
                Amount = 50.0   
            };

            var result = await _accountController.Transfer(request) as OkObjectResult;

            var response = result.Value as string;
            Assert.Contains("Transfer done successfully", response);

            var sourceAccount = await _bankingContext.Accounts.FirstOrDefaultAsync(a => a.AccountNumber ==  sourceAccountNumber);
            var targetAccount = await _bankingContext.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == targetAccountNumber);

            Assert.Equal(950.0, sourceAccount.Balance);
            Assert.Equal(1050.0, targetAccount.Balance);
        }

        [Fact]
        public async Task GetBalance_ShouldReturnBalance()
        {
            var result = await _accountController.GetBalance("1111456789012345") as OkObjectResult;

            var response = result.Value as string;
            Assert.Contains("Account balance is", response);
        }
    }
}
