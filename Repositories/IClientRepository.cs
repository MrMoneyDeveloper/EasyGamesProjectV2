using EasyGamesProjectV2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyGamesProjectV2.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetClients();
        Task<IEnumerable<Client>> GetClientsByPage(int page, int pageSize, string filter = "", string sort = "");
        Task<Client> GetClient(int id);
        Task AddClient(Client client);
        Task UpdateClient(Client client);
        Task DeleteClient(int id);
        Task AddTransaction(int clientId, decimal amount, int transactionTypeId, string comment);
    }
}
