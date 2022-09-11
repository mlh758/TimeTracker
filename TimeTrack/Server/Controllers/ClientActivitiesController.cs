using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Data;
using TimeTrack.Shared.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using VM = TimeTrack.Shared.ViewModels;

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
        public async Task<ICollection<VM.ClientActivity>> GetActivities(DateTime within)
        {
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));
            var startAt = new DateTime(within.Year, within.Month, 1);
            var endAt = startAt.AddDays(DateTime.DaysInMonth(startAt.Year, startAt.Month));
            return await _context.ClientActivities
                .Where(a => a.UserId == userId && a.Start >= startAt && a.Start <= endAt)
                .Include(a => a.Assessments)
                .Include(a => a.Client)
                .Select(a => new VM.ClientActivity()
                {
                    Start = a.Start,
                    End = a.End,
                    Client = new VM.ActivityClient() { Abbreviation = a.Client!.Abbreviation },
                    Assessments = a.Assessments!,

                }).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ClientActivity>> CreateClientActivity(NewActivity body)
        {
            if (body.Client is null || body.Assessments is null)
            {
                return BadRequest();
            }
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));
            var assessments = await _context.Assessments.Where(a => body.Assessments.Select(el => el.Id).Contains(a.Id)).ToListAsync();
            var newActivity = new ClientActivity
            {
                Start = body.Start,
                End = body.Start.AddMinutes(body.Duration),
                ClientId = body.Client.Id,
                UserId = userId,
                Assessments = assessments,
            };
            _context.Add(newActivity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClientActivity), new { id = newActivity.Id }, newActivity);
        }

    }
}
