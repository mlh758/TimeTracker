using TimeTrack.Shared;
using TimeTrack.Server.Models;
using TimeTrack.Server.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TimeTrack.Server.Repositories
{
    public interface IGroupRepository
    {
        public Task<Group?> Find(string userId, long id);
        public Task<List<Group>> All(string userId);
        public Task<Group> Create(string userId, GroupForm form);
        public Task<Group?> Update(string userId, long id, GroupForm form);
        public Task Destroy(string userId, long id);
    }
    public class GroupRepository : IGroupRepository
    {
        private readonly TimeContext _context;
        public GroupRepository(TimeContext context)
        {
            _context = context;
        }

        public async Task<List<Group>> All(string userId)
        {
            var query = from g in _context.Groups join cl in _context.Clients on g.Id equals cl.GroupId where cl.UserId == userId select g;
            return await query.ToListAsync();
        }

        public async Task<Group> Create(string userId, GroupForm form)
        {
            var clients = await LoadClients(userId, form);
            var group = new Group() { Clients = clients, Name = form.Name! };
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task Destroy(string userId, long id)
        {
            var group = await FindById(userId, id).FirstOrDefaultAsync();
            if (group is not null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Group?> Find(string userId, long id)
        {
            return await FindById(userId, id).Include(g => g.Clients).FirstOrDefaultAsync();
        }

        public async Task<Group?> Update(string userId, long id, GroupForm form)
        {
            var group = await FindById(userId, id).Include(g => g.Clients).FirstOrDefaultAsync();
            if (group is null)
            {
                return null;
            }
            group.Name = form.Name!;
            group.Clients = await LoadClients(userId, form);
            await _context.SaveChangesAsync();
            return group;
        }

        private IQueryable<Group> FindById(string userId, long id)
        {
            return from g in _context.Groups join cl in _context.Clients on g.Id equals cl.GroupId where cl.UserId == userId && g.Id == id select g;
        }

        private async Task<List<Models.Client>> LoadClients(string userId, GroupForm form)
        {
            var clientIds = form.Clients.Select(g => g.Id).ToList();
            return await _context.Clients.Where(c => c.UserId == userId && clientIds.Contains(c.Id)).ToListAsync();
        }
    }
}
