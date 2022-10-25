using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TimeTrack.Server.Data;
using TimeTrack.Shared;
using TimeTrack.Shared.ViewModels;
using VM = TimeTrack.Shared.ViewModels;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly TimeContext _context;
        public ClientsController(TimeContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VM.Client>> GetClient(int id)
        {
            var userId = HttpContext.User.FindFirstValue("UserID");
            var client = await forUser(Convert.ToInt32(userId), id);

            if (client == null) 
            {
                return NotFound();
            }

            return client;
        }

        private async Task<VM.Client?> forUser(int userId, int clientId)
        {
            var client = await _context
                .Clients
                .Where(c => c.UserId == userId && c.Id == clientId)
                .Include(c => c.Age)
                .Include(c => c.Race)
                .Include(c => c.Gender)
                .Include(c => c.Setting)
                .Include(c => c.SexualOrientation)
                .FirstOrDefaultAsync();
            if (client is null)
            {
                return null;
            }

            await _context.Entry(client).Collection(c => c.Disabilities!).LoadAsync();
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
            return await _context.Clients.Where(c => c.UserId == Convert.ToInt32(userId)).Select(c => new SummaryClient() { Abbreviation = c.Abbreviation, Id = c.Id}).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<SummaryClient>> CreateClient(NewClientForm clientData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // ensures the client is created against the current session
            var userId = HttpContext.User.FindFirstValue("UserID");
            var newClient = new Shared.Models.Client(clientData.Abbreviation!)
            {
                UserId = Convert.ToInt32(userId),
                AgeId = clientData.Age!.Id,
                SettingId = clientData.Setting!.Id,
                SexualOrientationId = clientData.SexualOrientation!.Id,
                GenderId = clientData.Gender!.Id,
                RaceId = clientData.Race!.Id
            };
            if (clientData.Disabilities is not null)
            {
                var disabilityIds = clientData.Disabilities.Select(d => d.Id).ToList();
                newClient.Disabilities = await _context.Categories.Where(c => c.Type == Shared.Models.CategoryType.Disability && disabilityIds.Contains(c.Id)).ToListAsync();
            }
            _context.Add(newClient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, new SummaryClient() { Id = newClient.Id, Abbreviation = newClient.Abbreviation });
        }

    }
}
