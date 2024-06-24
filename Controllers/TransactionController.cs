using Microsoft.AspNetCore.Mvc;
using EasyGamesProjectV2.Data;
using EasyGamesProjectV2.ViewModels;
using Dapper;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using EasyGamesProjectV2.Models;

namespace EasyGamesProjectV2.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly DapperContext _context;

        public TransactionController(DapperContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionViewModel model)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("ClientID", model.ClientID);
                parameters.Add("Amount", model.TransactionTypeID == 2 ? -model.Amount : model.Amount); // Make amount negative for credits
                parameters.Add("TransactionTypeID", model.TransactionTypeID);
                parameters.Add("Comment", dbType: DbType.String, direction: ParameterDirection.Output, size: 200);

                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.QueryAsync<dynamic>(
                        "EXEC spAddTransaction @ClientID, @Amount, @TransactionTypeID, @Comment OUT",
                        parameters);

                    // Check if result is not empty
                    if (result != null && result.Any())
                    {
                        var newBalance = result.FirstOrDefault()?.NewBalance;
                        return Ok(new { message = "Transaction added successfully", newBalance });
                    }
                    else
                    {
                        var message = parameters.Get<string>("Comment");
                        return BadRequest(new { message = message ?? "Transaction failed for an unknown reason" });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error adding transaction.", details = ex.Message, stackTrace = ex.StackTrace });
            }
        }


        [HttpGet("get-by-client/{clientId:int}")]
        public async Task<IActionResult> GetTransactionsByClient(int clientId)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var transactions = await connection.QueryAsync<TransactionViewModel>(
                        "EXEC spGetTransactionsByClientId @ClientID", new { ClientID = clientId });

                    if (transactions == null || !transactions.Any())
                        return NotFound();

                    return Ok(transactions);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error fetching transactions for client ID {clientId}.", details = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("update-comments")]
        public async Task<IActionResult> UpdateComments([FromBody] List<CommentUpdateViewModel> commentUpdates)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    foreach (var update in commentUpdates)
                    {
                        var parameters = new { update.TransactionID, update.Comment };
                        await connection.ExecuteAsync("UPDATE [Transaction] SET Comment = @Comment WHERE TransactionID = @TransactionID", parameters);
                    }
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating comments.", details = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
