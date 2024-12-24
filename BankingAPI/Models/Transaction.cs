using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingAPI.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string TransactionId { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionType { get; set; }
        public double Amount { get; set; }
        public DateTime Timestamp { get; set; }
        [ForeignKey("AccountNumber")]
        public Account Account { get; set; }
    }
}
