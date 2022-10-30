using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeTrack.Server.Repositories;
using TimeTrack.Shared;
using TimeTrack.Shared.ViewModels;
using M = TimeTrack.Shared.Models;
using VM = TimeTrack.Shared.ViewModels;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        public ClientsController(IClientRepository repo)
        {
            _clientRepository = repo;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VM.Client>> GetClient(int id)
        {
            var userId = HttpContext.User.FindFirstValue("UserID");
            var client = await _clientRepository.Find(Convert.ToInt32(userId), id);

            if (client == null) 
            {
                return NotFound();
            }

            return MapToView(client);
        }

        private VM.Client MapToView(M.Client client)
        {
            return new VM.Client()
            {
                Abbreviation = client.Abbreviation,
                Age = client.Age!,
                Race = client.Race!,
                Gender = client.Gender!,
                Setting = client.Setting!,
                SexualOrientation = client.SexualOrientation!,
                Disabilities = client.Disabilities!.Select(d => (VM.Category)d).ToList(),
            };

        }

        [HttpGet]
        public async Task<ICollection<SummaryClient>> GetClients()
        {
            var userId = HttpContext.User.FindFirstValue("UserID");
            var clients = await _clientRepository.All(Convert.ToInt32(userId));
            return clients.Select(c => new SummaryClient() { Abbreviation = c.Abbreviation, Id = c.Id }).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<SummaryClient>> CreateClient(NewClientForm clientData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // ensures the client is created against the current session
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));
            var newClient = await _clientRepository.Create(userId, clientData);
            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, new SummaryClient() { Id = newClient.Id, Abbreviation = newClient.Abbreviation });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClient(int id)
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));
            await _clientRepository.Destroy(userId, id);
            return Ok();
        }

    }
}
