using Dapper;
using EasyGamesProjectV2.Models;
using EasyGamesProjectV2.Data;
using EasyGamesProjectV2.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System;

namespace EasyGamesProjectV2.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DapperContext _context;

        public TransactionRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<dynamic>> AddTransaction(AddTransactionViewModel model)
        {
            var procedureName = "spAddTransaction";
            var parameters = new { model.ClientID, model.Amount, model.TransactionTypeID, model.Comment };
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.QueryAsync<dynamic>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding transaction.", ex);
            }
        }

        public async Task<IEnumerable<TransactionViewModel>> GetTransactionsByClientId(int clientId)
        {
            var procedureName = "spGetTransactionsByClientId";
            var parameters = new { ClientID = clientId };
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<TransactionViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error fetching transactions.", ex);
            }
        }
    }
}
