using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using EasyGamesProjectV2.Models;
using EasyGamesProjectV2.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyGamesProjectV2.Pages.Transaction
{
    public class EditTransactionModel : PageModel
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITransactionRepository _transactionRepository;

        public EditTransactionModel(IClientRepository clientRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _transactionRepository = transactionRepository;
        }

        [BindProperty(SupportsGet = true)]
        public int ClientId { get; set; }

        public string ClientName { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }
        public decimal TransactionTotal { get; set; }
        public decimal ClientBalance { get; set; }

        public async Task<IActionResult> OnGetAsync(int clientId)
        {
            ClientId = clientId;
            await LoadTransactionsAsync();
            return Page();
        }

        private async Task LoadTransactionsAsync()
        {
            var client = await _clientRepository.GetClient(ClientId);
            if (client == null)
            {
                // Handle the case when the client is not found
                Response.Redirect("/Client");
                return;
            }

            ClientName = $"{client.Name} {client.Surname}";

            Transactions = (await _transactionRepository.GetTransactionsByClientId(ClientId)).ToList();
            ClientBalance = Transactions.Sum(t => t.TransactionTypeName == "Credit" ? -t.Amount : t.Amount);
            TransactionTotal = Transactions.Sum(t => t.Amount);
        }
    }
}
