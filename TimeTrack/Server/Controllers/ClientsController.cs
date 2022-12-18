using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TimeTrack.Server.Repositories;
using TimeTrack.Shared;
using TimeTrack.Shared.ViewModels;
using M = TimeTrack.Server.Models;
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
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = await _clientRepository.Find(userId, id);

            if (client is null)
            {
                return NotFound();
            }

            return MapToView(client);
        }

        /*
         * If a custom value is set, show that. It's possible the user just set Other/Uknown and didn't provide
         * a custom value though so we still want to fall back to whatever is on the base Category column. Those are non-null
         * so SOMETHING will be there.
         */
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
        public async Task<ICollection<ActivityOwner>> GetClients()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var clients = await _clientRepository.All(userId);
            return clients.Select(c => new ActivityOwner() { Name = c.Abbreviation, Id = c.Id }).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<ActivityOwner>> CreateClient(NewClientForm clientData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newClient = await _clientRepository.Create(userId, clientData);
            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, new ActivityOwner() { Id = newClient.Id, Name = newClient.Abbreviation });
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ActivityOwner>> EditClient(int id, NewClientForm clientData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updatedClient = await _clientRepository.Update(userId, id, clientData);
            if (updatedClient is null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetClient), new { id = updatedClient.Id }, new ActivityOwner() { Id = updatedClient.Id, Name = updatedClient.Abbreviation });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClient(int id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _clientRepository.Destroy(userId, id);
            return Ok();
        }

    }
}
