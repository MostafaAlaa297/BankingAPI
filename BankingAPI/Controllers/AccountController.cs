using BankingAPI.Data;
using BankingAPI.Models;
using BankingAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Principal;

namespace BankingAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly BankingContext _context;
        
        public AccountController(BankingContext context)
        {
            _context = context;
        }

        [HttpPost("accounts/create")]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
        {
            var newAccount = new Account
            {
                AccountNumber = request.AccountNumber,
                AccountType = request.AccountType,
                Balance = 0.0,
                InterestRate = request.InterestRate,
                OverdraftLimit = request.OverdraftLimit
            };

            bool accountAlreadyExists = _context.Accounts.Any(a => a.AccountNumber == request.AccountNumber);

            if (accountAlreadyExists)
            {
                return BadRequest("Account already exists");
            }

            if (string.IsNullOrWhiteSpace(request.AccountNumber))
            {
                return BadRequest("Account number doesn't exist");
            }
            if (string.IsNullOrWhiteSpace(request.AccountType))
            {
                return BadRequest("Account Type not entered");
            }
            if (request.AccountType != "Savings" && request.AccountType != "Checking")
            {
                return BadRequest("Invalid Account Type. Available account types are Checking or Savings");
            }

            _context.Accounts.Add(newAccount);
            _context.SaveChanges();

            return Ok(newAccount);
        }

        [HttpPost("accounts/deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
        {
            Console.WriteLine($"Deposit request for AccountNumber: {request.AccountNumber}");
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == request.AccountNumber);
            if (account == null) return NotFound("Account not found");

            account.Balance += request.Amount;

            var transaction = new Transaction(account.AccountNumber, "Deposit", request.Amount, DateTime.UtcNow);
            
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return Ok(account.Balance);
        }

        [HttpPost("accounts/withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdraRequest request)
        {
            Console.WriteLine($"Withdraw request for AccountNumber: {request.AccountNumber}");
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == request.AccountNumber);
            if (account == null) return NotFound("Account not found");

            try
            {
                double NewBalance;
                if (account.AccountType == "Savings")
                {
                    var savingsAccount = new SavingsAccountService(0.05f, _context);
                    NewBalance = savingsAccount.Withdraw(account.AccountNumber, account.AccountType, request.Amount);
                }
                else if(account.AccountType == "Checking")
                {
                    var checkingAccount = new CheckingAccountService(500, _context);
                    NewBalance = checkingAccount.Withdraw(account.AccountNumber, account.AccountType, request.Amount);
                } else
                {
                    return BadRequest("Invalid Account Type");
                }
            } catch (InsufficientFundsException ex)
            {
                return BadRequest(ex.Message);
            }

            var transaction = new Transaction(account.AccountNumber, "Withdraw", request.Amount, DateTime.UtcNow);
            
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return Ok(account.Balance);
        }

        [HttpPost("accounts/transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
        {
            double[] result;
            try
            {
                if (request == null) 
                {
                    return BadRequest("Invalid request");
                }

                var bankSystem = new BankSystem(_context);
                result = bankSystem.TransferFunds(request.SourceAccountNumber, request.TargetAccountNumber, request.Amount);

            } catch (InsufficientFundsException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok($"Transfer done successfully. \nSource account balance: {result[0]} \nTarget account balance: {result[1]}");
        }
        [HttpGet("accounts/{id}/balance")]
        public async Task<IActionResult> GetBalance(string id)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == id);
            if (account == null) return NotFound("Account not found");

            return Ok($"Account balance is: {account.Balance} \nAccount Type: {account.AccountType}");
        }
    }
}
