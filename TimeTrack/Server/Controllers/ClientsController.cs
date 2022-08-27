using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Data;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
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
        public ICollection<Shared.Models.Client> GetClients()
        {
            return _context.Clients.ToArray();
        }

        [HttpPost]
        public async Task<ActionResult<Shared.Models.Client>> CreateClient(Shared.Models.Client newClient)
        {
            _context.Add(newClient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClient), new { id = newClient.Id }, newClient);
        }

    }
}
