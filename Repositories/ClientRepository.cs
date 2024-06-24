using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using EasyGamesProjectV2.Models;
using EasyGamesProjectV2.Data;
using System;
using Microsoft.Data.SqlClient;

namespace EasyGamesProjectV2.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DapperContext _context;

        public ClientRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            var procedureName = "spGetAllClients";
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<Client>(procedureName, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error fetching clients.", ex);
            }
        }

        public async Task<IEnumerable<Client>> GetClientsByPage(int page, int pageSize, string filter, string sort)
        {
            var offset = (page - 1) * pageSize;
            var sql = "SELECT * FROM Client WHERE (@Filter IS NULL OR Name LIKE '%' + @Filter + '%' OR Surname LIKE '%' + @Filter + '%') ORDER BY " +
                      "(CASE WHEN @Sort = 'firstNameAsc' THEN Name END) ASC, " +
                      "(CASE WHEN @Sort = 'firstNameDesc' THEN Name END) DESC, " +
                      "(CASE WHEN @Sort = 'lastNameAsc' THEN Surname END) ASC, " +
                      "(CASE WHEN @Sort = 'lastNameDesc' THEN Surname END) DESC, " +
                      "(CASE WHEN @Sort = 'balanceAsc' THEN ClientBalance END) ASC, " +
                      "(CASE WHEN @Sort = 'balanceDesc' THEN ClientBalance END) DESC " +
                      "OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<Client>(sql, new { Offset = offset, PageSize = pageSize, Filter = string.IsNullOrEmpty(filter) ? null : filter, Sort = sort });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error fetching clients by page.", ex);
            }
        }

        public async Task<Client> GetClient(int id)
        {
            var procedureName = "spGetClientByID";
            var parameters = new { ClientID = id };
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QuerySingleOrDefaultAsync<Client>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching client with ID {id}.", ex);
            }
        }

        public async Task AddClient(Client client)
        {
            var procedureName = "spAddClient";
            var parameters = new { client.Name, client.Surname, client.ClientBalance };
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding client.", ex);
            }
        }

        public async Task UpdateClient(Client client)
        {
            var procedureName = "spUpdateClient";
            var parameters = new { client.ClientID, client.Name, client.Surname, client.ClientBalance };
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating client.", ex);
            }
        }

        public async Task DeleteClient(int id)
        {
            var procedureName = "spDeleteClient";
            var parameters = new { ClientID = id };
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException ex) when (ex.Number == 547)  // Foreign key violation error number
            {
                throw new InvalidOperationException($"Cannot delete client with ID {id} because there are related transactions.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting client with ID {id}.", ex);
            }
        }

        public async Task AddTransaction(int clientId, decimal amount, int transactionTypeId, string comment)
        {
            var procedureName = "spAddTransaction";
            var parameters = new { ClientID = clientId, Amount = amount, TransactionTypeID = transactionTypeId, Comment = comment };
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error adding transaction.", ex);
            }
        }
    }
}
