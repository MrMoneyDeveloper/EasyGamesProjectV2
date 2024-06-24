using System;

namespace EasyGamesProjectV2.Models
{
    public class TransactionViewModel
    {
        public long TransactionID { get; set; }
        public decimal Amount { get; set; }
        public string TransactionTypeName { get; set; }
        public string Comment { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
