using System.ComponentModel.DataAnnotations;

namespace EasyGamesProjectV2.Models
{
    public class TransactionType
    {
        public int TransactionTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionTypeName { get; set; }
    }
}