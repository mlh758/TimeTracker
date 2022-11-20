using TimeTrack.Server.Data;
using M = TimeTrack.Server.Models;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Shared;
using TimeTrack.Shared.Enums;


namespace TimeTrack.Server.Repositories
{
    public interface IClientRepository
    {
        public Task<M.Client?> Find(string userId, int clientId);
        public Task<List<M.Client>> All(string userId);
        public Task<M.Client> Create(string userId, NewClientForm form);
        public Task<M.Client?> Update(string userId, int clientId, NewClientForm form);
        public Task Destroy(string userId, int clientId);
    }
    /*
     * There's a lot of non-null assert and casting here. It should be safe because
     * we validate on model state and the key for category is a 32 bit int. There's
     * probably a more type safe way to handle all this though.
     */
    public class ClientRepository : IClientRepository
    {

        private readonly TimeContext _context;
        public ClientRepository(TimeContext context)
        {
            _context = context;
        }

        public async Task<M.Client> Create(string userId, NewClientForm clientData)
        {
            var newClient = new M.Client(clientData.Abbreviation!)
            {
                UserId = userId,
                AgeId = (int)clientData.Age!.Id,
                SettingId = (int)clientData.Setting!.Id,
                SexualOrientationId = (int)clientData.SexualOrientation!.Id,
                GenderId = (int)clientData.Gender!.Id,
                RaceId = (int)clientData.Race!.Id
            };
            if (clientData.Disabilities is not null)
            {
                var disabilityIds = clientData.Disabilities.Select(d => d.Id).ToList();
                newClient.Disabilities = await _context.Categories.Where(c => c.Type == CategoryType.Disability && disabilityIds.Contains(c.Id)).ToListAsync();
            }
            _context.Add(newClient);
            await _context.SaveChangesAsync();
            return newClient;
        }

        public async Task<M.Client?> Update(string userId, int clientId, NewClientForm clientData)
        {
            var client = await _context.Clients.Where(c => c.UserId == userId && c.Id == clientId).FirstOrDefaultAsync();
            if (client is null)
            {
                return null;
            }
            await _context.Entry(client).Collection(c => c.Disabilities!).LoadAsync();
            client.AgeId = (int)clientData.Age!.Id;
            client.SettingId = (int)clientData.Setting!.Id;
            client.SexualOrientationId = (int)clientData.SexualOrientation!.Id;
            client.GenderId = (int)clientData.Gender!.Id;
            client.RaceId = (int)clientData.Race!.Id;

            var disabilities = clientData.Disabilities!.Select(d => d.Id).ToList();
            client.Disabilities = await _context.Categories.Where(c => c.Type == CategoryType.Disability && disabilities.Contains(c.Id)).ToListAsync();

            await _context.SaveChangesAsync();
            return client;

        }

        public async Task<M.Client?> Find(string userId, int clientId)
        {
            var client = await _context
            .Clients
                .Where(c => c.UserId == userId && c.Id == clientId)
                .Include(c => c.Age)
                .Include(c => c.Race)
                .Include(c => c.Gender)
                .Include(c => c.Setting)
                .Include(c => c.SexualOrientation)
                .FirstOrDefaultAsync();
            if (client is null)
            {
                return null;
            }

            await _context.Entry(client).Collection(c => c.Disabilities!).LoadAsync();
            return client;
        }

        public async Task<List<M.Client>> All(string userId)
        {
            return await _context.Clients.Where(c => c.UserId == userId).ToListAsync();
        }

        async Task IClientRepository.Destroy(string userId, int clientId)
        {
            var client = await _context.Clients.Where(c => c.UserId == userId && c.Id == clientId).FirstOrDefaultAsync();
            if (client is not null)
            {
                _context.Remove(client);
                await _context.SaveChangesAsync();
            }
        }
    }
}
