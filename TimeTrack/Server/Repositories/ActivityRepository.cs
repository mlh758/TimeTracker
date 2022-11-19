using TimeTrack.Server.Data;
using TimeTrack.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Server.Services;

namespace TimeTrack.Server.Repositories
{
    public interface IActivityRepository
    {
        public Task<List<Activity>> ForUserWithin(string userId, DateTime start, DateTime end);
        public Task<Activity?> Find(string userId, int Id);
        public Task<Activity> Create(Activity activity);

        public Task<int> CreateScheduled(Activity activity);
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


        public async Task<Activity?> Find(string userId, int Id)
        {
            return await userActivity(userId).Where(a => a.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<List<Activity>> ForUserWithin(string userId, DateTime start, DateTime end)
        {
            var activity = userActivity(userId).Where(a => a.Start >= start && a.Start <= end).Include(a => a.Assessments).Include(a => a.Client);
            return await activity.ToListAsync();
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

        private IQueryable<Activity> userActivity(string userId)
        {
            return from u in _context.Users
                   join cl in _context.Clients on u.Id equals cl.UserId
                   join a in _context.Activities on cl.Id equals a.ClientId
                   where u.Id == userId
                   select a;
        }
    }
}
