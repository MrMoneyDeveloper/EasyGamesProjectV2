using System.ComponentModel.DataAnnotations;

namespace EasyGamesProjectV2.Models
{
    public class Client
    {
        public int ClientID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a non-negative value.")]
        public decimal ClientBalance { get; set; }
    }
}








