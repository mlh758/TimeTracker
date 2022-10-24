using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TimeTrack.Server.Data;
using TimeTrack.Shared;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private TimeContext _context;
        public ClientsController(TimeContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shared.Models.Client>> GetClient(long id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null) 
            {
                return NotFound();
            }

            return client;
        }

        [HttpGet]
        public async Task<ICollection<Shared.Models.Client>> GetClients()
        {
            var userId = HttpContext.User.FindFirstValue("UserID");
            return await _context.Clients.Where(c => c.UserId == Convert.ToInt32(userId)).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Shared.Models.Client>> CreateClient(NewClientForm clientData)
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
                AgeId = clientData.Age.Id,
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
            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, newClient);
        }

    }
}
