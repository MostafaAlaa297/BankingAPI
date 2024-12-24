using BankingAPI.Data;
using BankingAPI.Models;
using BankingAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BankingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly BankingContext _context;
        
        public AccountController(BankingContext context)
        {
            _context = context;
        }

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] DepositRequest request)
        {
            Console.WriteLine($"Deposit request for AccountNumber: {request.AccountNumber}");
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNumber == request.AccountNumber);
            if (account == null) return NotFound("Account not found");

            account.Balance += request.Amount;

            var transaction = new Transaction
            {
                AccountNumber = account.AccountNumber,
                TransactionType = "Deposit",
                Amount = request.Amount,
                Timestamp = DateTime.UtcNow
            };
            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return Ok(new { account.Balance });
        }
        
    }
}
