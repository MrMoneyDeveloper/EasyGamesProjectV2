using EasyGamesProjectV2.Models;
using EasyGamesProjectV2.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyGamesProjectV2.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionViewModel>> GetTransactionsByClientId(int clientId);
        Task<IEnumerable<dynamic>> AddTransaction(AddTransactionViewModel model);
    }
}
