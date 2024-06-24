using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using EasyGamesProjectV2.Models;
using EasyGamesProjectV2.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EasyGamesProjectV2.Pages.Transaction
{
    public class AddTransactionModel : PageModel
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITransactionRepository _transactionRepository;

        public AddTransactionModel(IClientRepository clientRepository, ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _transactionRepository = transactionRepository;
        }

        [BindProperty(SupportsGet = true)]
        public int ClientID { get; set; }

        public string ClientName { get; set; }
        public decimal ClientBalance { get; set; }
        public List<TransactionViewModel> PreviousTransactions { get; set; }

        public async Task<IActionResult> OnGetAsync(int clientId)
        {
            var client = await _clientRepository.GetClient(clientId);
            if (client == null)
            {
                return NotFound();
            }
            ClientID = clientId;
            ClientName = $"{client.Name} {client.Surname}";
            ClientBalance = client.ClientBalance;
            PreviousTransactions = (List<TransactionViewModel>)await _transactionRepository.GetTransactionsByClientId(clientId);

            return Page();
        }
    }
}
