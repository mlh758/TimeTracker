using TimeTrack.Server.Data;
using TimeTrack.Server.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Server.Services;
using TimeTrack.Shared.Enums;
using TimeTrack.Shared;
using static MudBlazor.CategoryTypes;

namespace TimeTrack.Server.Repositories
{
    public interface IActivityRepository
    {
        public Task<List<Activity>> ForUserWithin(string userId, DateTime start, DateTime end);
        public Task<Activity?> Find(string userId, long Id);
        public Task<Activity> Create(Activity activity);

        public Task<int> CreateScheduled(Activity activity);
        public Task Delete(string userId, long Id, ActivityDelete mode);
        public Task Update(string userId, long Id, ActivityForm properties);
    }
    public class ActivityRepository : IActivityRepository
    {

        private readonly TimeContext _context;
        public ActivityRepository(TimeContext context)
        {
            _context = context;
        }

        public async Task<Activity> Create(Activity activity)
        {

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }


        public async Task<Activity?> Find(string userId, long Id)
        {
            var includeNested = (IQueryable<Activity> a) => a.Where(a => a.Id == Id).Include(a => a.Client).Include(a => a.Group).Include(a => a.Assessments).Include(a => a.Schedule).FirstOrDefaultAsync();
            var clientActivity = await includeNested(ClientUserActivity(userId));
            if (clientActivity is not null)
            {
                return clientActivity;
            }
            return await includeNested(GroupUserActivity(userId));
        }

        public async Task<List<Activity>> ForUserWithin(string userId, DateTime start, DateTime end)
        {
            var activity = await ClientUserActivity(userId).Where(a => a.Start >= start && a.Start <= end).Include(a => a.Assessments).Include(a => a.Client).Select(a => a).ToListAsync();
            var groups = await GroupUserActivity(userId).Where(a => a.Start >= start && a.Start <= end).Include(a => a.Assessments).Include(a => a.Group).Select(a => a).ToListAsync();
            return activity.Concat(groups).OrderBy(a => a.Start).ToList();
        }

        public async Task<int> CreateScheduled(Activity activity)
        {
            if (activity.Schedule is null)
            {
                throw new ArgumentException("creating a scheduled activity requires a schedule");
            }
            _context.Schedules.Add(activity.Schedule);
            await _context.SaveChangesAsync();
            var generator = new ScheduleGenerator(DateOnly.FromDateTime(activity.Start), activity.Schedule);
            var activities = generator.Dates().Select(d => new Activity(activity) { Start = d.ToDateTime(TimeOnly.MinValue) });
            var count = 0;
            foreach (var a in activities)
            {
                count++;
                _context.Activities.Add(a);
            }
            await _context.SaveChangesAsync();
            return count;
        }

        public async Task Update(string userId, long Id, ActivityForm properties)
        {
            var selectedIds = properties.Assessments.Select(a => a.Id);
            var selectedAssessments = selectedIds.Any() ? await _context.Assessments.Where(a => selectedIds.Contains(a.Id)).ToListAsync() : Enumerable.Empty<Assessment>();
            var existing = await Find(userId, Id);
            if (existing is null)
            {
                return;
            }
            // We should update all the items in the schedule if we update a scheduled activity. Make sure to maintain the original start date.
            if (existing.ScheduleId.HasValue)
            {
                foreach (var item in _context.Activities.Where(a => a.ScheduleId == existing.ScheduleId))
                {
                    var originalDate = item.Start;
                    ActivityFactory.UpdateFromForm(item, properties, selectedAssessments);
                    item.Start = originalDate;
                }
            } else
            {
                ActivityFactory.UpdateFromForm(existing, properties, selectedAssessments);
            }
            await _context.SaveChangesAsync();
        }

        private IQueryable<Activity> ClientUserActivity(string userId)
        {
            return from cl in _context.Clients
                   join a in _context.Activities on cl.Id equals a.ClientId
                   where cl.UserId == userId
                   select a;
        }

        private IQueryable<Activity> GroupUserActivity(string userId)
        {
            return from a in _context.Activities
                   join g in _context.Groups on a.GroupId equals g.Id
                   where g.UserId == userId
                   select a;
        }

        public async Task Delete(string userId, long Id, ActivityDelete mode)
        {
            var saved = await Find(userId, Id);
            if (saved is null)
            {
                return;
            }
            if (!saved.ScheduleId.HasValue || mode == ActivityDelete.This)
            {
                _context.Activities.Remove(saved);
                await _context.SaveChangesAsync();
                return;
            }
            IQueryable<Activity> query;
            if (mode == ActivityDelete.Future)
            {
                query = _context.Activities.Where(a => a.ScheduleId == saved.ScheduleId && a.Start >= saved.Start);
            } else
            {
                query = _context.Activities.Where(a => a.ScheduleId == saved.ScheduleId);
            }
            _context.Activities.RemoveRange(query);
            await _context.SaveChangesAsync();
        }
    }
}
