using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Data;
using TimeTrack.Shared.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using VM = TimeTrack.Shared.ViewModels;
using TimeTrack.Server.Repositories;
using System.Linq;
using TimeTrack.Shared;

namespace TimeTrack.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientActivitiesController : ControllerBase
    {
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
                Duration = a.Duration,
                Client = new VM.SummaryClient() { Abbreviation = a.Client!.Abbreviation, Id = a.ClientId },
                Assessments = a.Assessments!.Select(a => new VM.Assessment { Name = a.Name }).ToList(),

            }).ToList();
        }

        [HttpPost]
        public async Task<ActionResult> CreateClientActivity(ActivityForm body)
        {
            if (body.Client is null || body.Assessments is null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            var userId = Convert.ToInt32(HttpContext.User.FindFirstValue("UserID"));
            var assessments = await _assessmentRepo.FindById(body.Assessments.Select(el => el.Id));
            var newActivity = new Activity
            {
                Start = body.Start!.Value!,
                Duration = body.Duration!.Value,
                ClientId = body.Client.Id,
                Schedule = body.Schedule,
                Assessments = assessments.ToList(),
            };
            if (newActivity.Schedule is null)
            {
                newActivity = await _activityRepo.Create(newActivity);
                return CreatedAtAction(nameof(GetClientActivity), new { id = newActivity.Id }, newActivity);

            } else
            {
                await _activityRepo.CreateScheduled(newActivity);
                return CreatedAtAction(nameof(GetActivities), null);
            }
        }

    }
}
