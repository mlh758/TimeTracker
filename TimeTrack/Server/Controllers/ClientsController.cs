using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TimeTrack.Server.Data;

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
        public async Task<ActionResult<Shared.Models.Client>> CreateClient(Shared.Models.Client newClient)
        {
            // ensures the client is created against the current session
            var userId = HttpContext.User.FindFirstValue("UserID");
            newClient.UserId = Convert.ToInt32(userId);
            _context.Add(newClient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, newClient);
        }

    }
}
