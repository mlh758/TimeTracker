using TimeTrack.Server.Data;
using TimeTrack.Shared.Enums;
using System.Linq;
using VM = TimeTrack.Shared.ViewModels;
using TimeTrack.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace TimeTrack.Server.Services
{
    public class Demographer
    {
        private class CategoryCounter
        {
            private Dictionary<VM.Category, int> _counts;
            public CategoryCounter()
            {
                _counts= new Dictionary<VM.Category, int>();
            }

            public void Add(VM.Category cat)
            {
                _counts.TryGetValue(cat, out int existingCount);
                _counts[cat] = existingCount + 1;
            }

            public Dictionary<VM.Category, int> Result()
            {
                return _counts;
            }
        }
        private readonly TimeContext _context;

        public Demographer(TimeContext context)
        {
            _context = context;
        }

        // Assumes that the group's clients are preloaded along with those clients' disabilities
        public async Task<Dictionary<VM.Category, int>> Gather(Group group, string userId)
        {
            var categories = await GetCategories(userId);
            var counts = new CategoryCounter();
            foreach (var client in group.Clients!)
            {
                counts.Add(categories[client.AgeId]);
                counts.Add(categories[client.RaceId]);
                counts.Add(categories[client.SexualOrientationId]);
                counts.Add(categories[client.GenderId]);
                counts.Add(categories[client.SettingId]);
                foreach (var disability in client.Disabilities!)
                {
                    counts.Add(disability);
                }
            }
            return counts.Result();
        }


        // There are about 50 categories total, users will probably have just a few of their own. Just pull them
        // all into memory.
        private async Task<Dictionary<long, VM.Category>> GetCategories(string userId)
        {
            var query = from c in _context.Categories where c.UserId == null || c.UserId == userId select new VM.Category(c.Name) { Id = c.Id, Type = c.Type };
            return await query.ToDictionaryAsync(c => c.Id, c => c);
        }
    }
}
