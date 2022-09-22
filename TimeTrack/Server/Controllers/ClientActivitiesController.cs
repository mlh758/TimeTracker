using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Data;
using TimeTrack.Shared.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using VM = TimeTrack.Shared.ViewModels;
using TimeTrack.Server.Repositories;
using System.Linq;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientActivitiesController : ControllerBase
    {
        public class NewActivity : Activity
        {
            public int Duration { get; set; }
        }
        private readonly IActivityRepository _activityRepo;
        private readonly IAssessmentRepository _assessmentRepo;
        public ClientActivitiesController(IActivityRepository activityRepo, IAssessmentRepository assessmentRepository)
        {
            _activityRepo = activityRepo;
            _assessmentRepo = assessmentRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetClientActivity(int id)
        {
            var userId = HttpContext.User.FindFirstValue("UserID");
            var activity = await _activityRepo.Find(Convert.ToInt32(userId), id);

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
            var activities = await _activityRepo.ForUserWithin(userId, startAt, endAt);
            return activities.Select(a => new VM.ClientActivity()
            {
                Start = a.Start,
                End = a.End,
                Client = new VM.Client() { Abbreviation = a.Client!.Abbreviation },
                Assessments = a.Assessments!.Select(a => new VM.Assessment { Name = a.Name }).ToList(),

            }).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<Activity>> CreateClientActivity(NewActivity body)
        {
            if (body.Client is null || body.Assessments is null)
            {
                return BadRequest();
            }
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));
            var assessments = await _assessmentRepo.FindById(body.Assessments.Select(el => el.Id));
            var newActivity = new Activity
            {
                Start = body.Start,
                End = body.Start.AddMinutes(body.Duration),
                ClientId = body.Client.Id,
                Assessments = assessments.ToList(),
            };
            newActivity = await _activityRepo.Create(newActivity);
            return CreatedAtAction(nameof(GetClientActivity), new { id = newActivity.Id }, newActivity);
        }

    }
}
