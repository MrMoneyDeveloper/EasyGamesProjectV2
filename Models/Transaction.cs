using System.ComponentModel.DataAnnotations;

namespace EasyGamesProjectV2.Models
{
    public class Transaction
    {
        public long TransactionID { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a non-negative value.")]
        public decimal Amount { get; set; }

        [Required]
        public int TransactionTypeID { get; set; }

        [Required]
        public int ClientID { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }
    }
}
