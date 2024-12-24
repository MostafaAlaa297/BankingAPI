using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BankingAPI.Models;
namespace BankingAPI.Data
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options) : base(options) { }

        public DbSet<Account>? Accounts { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>()
                .HasKey(a => a.AccountNumber);

            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionId);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany()
                .HasForeignKey(t => t.AccountNumber);
        }
    }
}
