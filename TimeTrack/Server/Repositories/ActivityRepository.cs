using TimeTrack.Server.Data;
using TimeTrack.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TimeTrack.Server.Repositories
{
    public interface IActivityRepository
    {
        public Task<List<Activity>> ForUserWithin(int userId, DateTime start, DateTime end);
        public Task<Activity?> Find(int userId, int Id);
        public Task<Activity> Create(Activity activity);
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

        public async Task<Activity?> Find(int userId, int Id)
        {
            return await userActivity(userId).Where(a => a.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<List<Activity>> ForUserWithin(int userId, DateTime start, DateTime end)
        {
            var activity = userActivity(userId).Where(a => a.Start >= start && a.End <= end).Include(a => a.Assessments).Include(a => a.Client);
            return await activity.ToListAsync();
        }


        private IQueryable<Activity> userActivity(int userId)
        {
            return from u in _context.Users
                   join cl in _context.Clients on u.Id equals cl.UserId
                   join a in _context.Activities on cl.Id equals a.ClientId
                   where u.Id == userId
                   select a;
        }
    }
}
