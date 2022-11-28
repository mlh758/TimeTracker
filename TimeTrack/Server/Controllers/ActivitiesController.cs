using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeTrack.Server.Services;
using TimeTrack.Server.Models;
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
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityRepository _activityRepo;
        private readonly IAssessmentRepository _assessmentRepo;
        private readonly IClientRepository _clientRepo;
        public ActivitiesController(IActivityRepository activityRepo, IAssessmentRepository assessmentRepository, IClientRepository clientRepository)
        {
            _activityRepo = activityRepo;
            _assessmentRepo = assessmentRepository;
            _clientRepo = clientRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetClientActivity(int id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var activity = await _activityRepo.Find(userId, id);

            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        [HttpGet]
        public async Task<ICollection<VM.SummaryActivity>> GetActivities(DateTime within)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var startAt = new DateTime(within.Year, within.Month, 1);
            var endAt = startAt.AddDays(DateTime.DaysInMonth(startAt.Year, startAt.Month));
            var activities = await _activityRepo.ForUserWithin(userId, startAt, endAt);
            return activities.Select(a => new VM.SummaryActivity()
            {
                Start = a.Start,
                Duration = a.Duration,
                Owner = a.GetOwner(),
                Assessments = a.Assessments!.Select(a => new VM.Assessment { Name = a.Name }).ToList(),

            }).ToList();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ActivityForm body)
        {
            if (body.Client is null || body.Assessments is null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            // verify the client that is having activity generated belongs to the current user
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = await _clientRepo.Find(userId, body.Client.Id);
            if (client is null)
            {
                return NotFound();
            }
            var assessments = await _assessmentRepo.FindById(body.Assessments.Select(el => el.Id));
            var newActivity = ActivityFactory.FromForm(body, assessments);
            if (body.Schedule is null)
            {
                newActivity = await _activityRepo.Create(newActivity);
                return CreatedAtAction(nameof(GetClientActivity), new { id = newActivity.Id }, newActivity);

            }
            else
            {
                newActivity.Schedule = body.Schedule;
                await _activityRepo.CreateScheduled(newActivity);
                return CreatedAtAction(nameof(GetActivities), null);
            }
        }

    }
}
