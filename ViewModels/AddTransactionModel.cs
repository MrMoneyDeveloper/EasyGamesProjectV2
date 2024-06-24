using System.ComponentModel.DataAnnotations;

namespace EasyGamesProjectV2.ViewModels
{
    public class AddTransactionViewModel
    {
        [Required]
        public int ClientID { get; set; }

        // Remove the range attribute to allow for negative values for credit transactions
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int TransactionTypeID { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }
    }
}
