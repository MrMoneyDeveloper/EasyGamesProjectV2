using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EasyGamesProjectV2.Models;
using EasyGamesProjectV2.Repositories;
using System;

namespace EasyGamesProjectV2.Controllers
{
    [ApiController]
    [Route("api/client")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetClients(int page = 1, int pageSize = 10, string filter = "", string sort = "")
        {
            try
            {
                var clients = await _clientRepository.GetClientsByPage(page, pageSize, filter, sort);
                if (clients == null || !clients.Any())
                {
                    return NotFound(new { message = "No clients found" });
                }
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error fetching clients.", details = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            try
            {
                var client = await _clientRepository.GetClient(id);
                if (client == null)
                    return NotFound();

                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error fetching client with ID {id}.", details = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddClient(Client client)
        {
            try
            {
                await _clientRepository.AddClient(client);
                return CreatedAtAction(nameof(GetClient), new { id = client.ClientID }, client);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error adding client.", details = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, Client client)
        {
            if (id != client.ClientID)
                return BadRequest();

            try
            {
                await _clientRepository.UpdateClient(client);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating client.", details = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _clientRepository.GetClient(id);
            if (client == null)
            {
                return NotFound(new { message = $"Client with ID {id} not found." });
            }

            try
            {
                await _clientRepository.DeleteClient(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error deleting client with ID {id}.", details = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
