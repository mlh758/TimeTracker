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
using TimeTrack.Shared.Enums;

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
        private readonly IGroupRepository _groupRepository;
        public ActivitiesController(IActivityRepository activityRepo, IAssessmentRepository assessmentRepository, IClientRepository clientRepository, IGroupRepository groupRepository)
        {
            _activityRepo = activityRepo;
            _assessmentRepo = assessmentRepository;
            _clientRepo = clientRepository;
            _groupRepository = groupRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityForm>> GetClientActivity(long id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var activity = await _activityRepo.Find(userId, id);

            if (activity == null)
            {
                return NotFound();
            }

            var form = new ActivityForm()
            {
                Start = activity.Start,
                ClinicalHours = activity.ClinicalHours,
                Assessments = activity.Assessments!.Select(a => new VM.Assessment() { Id = a.Id, Name = a.Name }).ToHashSet(),
                UsedInIntegratedReport = activity.UsedInIntegratedReport,
                UsedInResearch = activity.UsedInResearch,
                ClinicallyScored = activity.ClinicallyScored,
            };
            if (activity.Group is not null)
            {
                form.Group = activity.Group;
            }
            if (activity.Client is not null)
            {
                form.Client = activity.Client;
            }
            if (activity.Schedule is not null)
            {
                form.Schedule = new VM.Schedule()
                {
                    Id = activity.Schedule.Id,
                    EndSchedule = activity.Schedule.EndSchedule,
                    DaysOfWeek = activity.Schedule.DaysOfWeek,
                    Interval = activity.Schedule.Interval,
                    Frequency = activity.Schedule.Frequency,
                };
            }
            return form;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, ActivityForm form)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _activityRepo.Update(userId, id, form);
            return Ok();
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
                Id = a.Id,
                Start = a.Start,
                ClinicalHours = a.ClinicalHours,
                Owner = a.GetOwner(),
                Assessments = a.Assessments!.Select(a => new VM.Assessment { Name = a.Name }).ToList(),
                IsScheduled = a.ScheduleId.HasValue,
            }).ToList();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ActivityForm body)
        {
            if (body.Assessments is null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (body.Client is not null)
            {
                // verify the client that is having activity generated belongs to the current user
                var client = await _clientRepo.Find(userId, body.Client.Id);
                if (client is null)
                {
                    return NotFound("Unable to locate client");
                }
            }
            if (body.Group is not null)
            {
                var group = await _groupRepository.Find(userId, body.Group.Id);
                if (group is null)
                {
                    return NotFound("Unable to locate group");
                }
            }
            var assessments = await _assessmentRepo.FindById(body.Assessments.Select(el => el.Id));
            var newActivity = ActivityFactory.FromForm(body, assessments);
            if (body.Schedule is null)
            {
                newActivity = await _activityRepo.Create(newActivity);
                return CreatedAtAction(nameof(GetClientActivity), new { id = newActivity.Id }, null);

            }
            else
            {
                newActivity.Schedule = body.Schedule;
                await _activityRepo.CreateScheduled(newActivity);
                return CreatedAtAction(nameof(GetActivities), null);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id, ActivityDelete mode)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _activityRepo.Delete(userId, id, mode);
            return Ok();
        }

    }
}
