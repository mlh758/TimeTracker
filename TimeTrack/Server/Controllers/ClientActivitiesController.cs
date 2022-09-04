using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Data;
using TimeTrack.Shared.Models;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ClientActivitiesController : ControllerBase
    {
        public class NewActivity : ClientActivity
        {
            public int Duration { get; set; }
        }
        private TimeContext _context;
        public ClientActivitiesController(TimeContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientActivity>> GetClientActivity(long id)
        {
            var activity = await _context.ClientActivities.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        [HttpGet]
        public ICollection<ClientActivity> GetClients()
        {
            return _context.ClientActivities.ToArray();
        }

        [HttpPost]
        public async Task<ActionResult<ClientActivity>> CreateClientActivity(NewActivity body)
        {
            if (body.Client is null)
            {
                return BadRequest();
            }
            var newActivity = new ClientActivity
            {
                Start = body.Start,
                End = body.Start.AddMinutes(body.Duration),
                ClientId = body.Client.Id,
                UserId = _context.Users.First().Id, // obviously wrong, we need auth set up to track users though
            };
            _context.Add(newActivity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClientActivity), new { id = newActivity.Id }, newActivity);
        }

    }
}
