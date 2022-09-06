using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Data;
using TimeTrack.Shared.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
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
            var userId = HttpContext.User.FindFirstValue("UserID");
            var activity = await _context.ClientActivities.Where(a => a.UserId == Convert.ToInt32(userId) && a.Id == id).AsNoTracking().FirstAsync();

            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        [HttpGet]
        public async Task<ICollection<ClientActivity>> GetClients()
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));
            return await _context.ClientActivities.Where(a => a.UserId == userId).Include(a => a.Client).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ClientActivity>> CreateClientActivity(NewActivity body)
        {
            if (body.Client is null)
            {
                return BadRequest();
            }
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));
            var newActivity = new ClientActivity
            {
                Start = body.Start,
                End = body.Start.AddMinutes(body.Duration),
                ClientId = body.Client.Id,
                UserId = userId,
            };
            _context.Add(newActivity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClientActivity), new { id = newActivity.Id }, newActivity);
        }

    }
}
