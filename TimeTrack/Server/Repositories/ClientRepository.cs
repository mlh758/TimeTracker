using TimeTrack.Server.Data;
using M = TimeTrack.Server.Models;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Shared;
using TimeTrack.Shared.Enums;


namespace TimeTrack.Server.Repositories
{
    /*
     * The NewClientForm provides validations to ensure the categories are not null.
     * Those should be checked before passing it into the repository which is going
     * to assume they are not null.
     */
    public interface IClientRepository
    {
        public Task<M.Client?> Find(string userId, long clientId);
        public Task<List<M.Client>> All(string userId);
        public Task<M.Client> Create(string userId, NewClientForm form);
        public Task<M.Client?> Update(string userId, long clientId, NewClientForm form);
        public Task Destroy(string userId, long clientId);
    }
    /*
     * There is probably a more type safe way to deal with all the null assertions. Should probably
     * parse into a version of the form where all fields are non-null in the controller or something.
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
            var newClient = new M.Client(clientData.Abbreviation!) {  UserId= userId };
            await AssignCategories(newClient, clientData);
            _context.Add(newClient);
            await _context.SaveChangesAsync();
            return newClient;
        }

        public async Task<M.Client?> Update(string userId, long clientId, NewClientForm clientData)
        {
            var client = await _context.Clients.Where(c => c.UserId == userId && c.Id == clientId).FirstOrDefaultAsync();
            if (client is null)
            {
                return null;
            }
            await _context.Entry(client).Collection(c => c.Disabilities!).LoadAsync();
            await AssignCategories(client, clientData);

            await _context.SaveChangesAsync();
            return client;

        }

        // If the user chose a custom category, set that category type to Other and assign the custom ID
        // Most of this is tedious copy pasta, surely there's a better way? Reflection is slow and we lose type
        // safety.
        private async Task AssignCategories(M.Client client, NewClientForm form)
        {
            var otherMap = await _context.Categories.Where(c => c.Name == "Other" || c.Name == "Unknown").ToDictionaryAsync(c => c.Type, c => c.Id);
            if (form.Race is not null && form.Race.IsCustom)
            {
                client.RaceId = otherMap[CategoryType.Race];
                client.CustomRaceId = form.Race.Id;
            }
            else
            {
                client.RaceId = form.Race!.Id;
            }
 
            if (form.Gender is not null && form.Gender.IsCustom)
            {
                client.GenderId = otherMap[CategoryType.Gender];
                client.CustomGenderId = form.Gender.Id;
            }
            else
            {
                client.GenderId = form.Gender!.Id;
            }

            if (form.SexualOrientation is not null && form.SexualOrientation.IsCustom)
            {
                client.SexualOrientationId = otherMap[CategoryType.SexualOrientation];
                client.CustomSexualOrientationId = form.SexualOrientation.Id;
            }
            else
            {
                client.SexualOrientationId = form.SexualOrientation!.Id;
            }

            if (form.Setting is not null && form.Setting.IsCustom)
            {
                client.SettingId = otherMap[CategoryType.TreatmentSetting];
                client.CustomSettingId = form.Setting.Id;
            }
            else
            {
                client.SettingId = form.Setting!.Id;
            }

            if (form.Age is not null && form.Age.IsCustom)
            {
                client.AgeId = otherMap[CategoryType.Age];
                client.CustomAgeId = form.Age.Id;
            }
            else
            {
                client.AgeId = form.Age!.Id;
            }
            await AssignDisabilities(client, form);
        }

        private async Task AssignDisabilities(M.Client client, NewClientForm form)
        {
            var disabilities = form.Disabilities.Where(d => !d.IsCustom).Select(d => d.Id).ToList();
            client.Disabilities = await _context.Categories.Where(c => c.Type == CategoryType.Disability && disabilities.Contains(c.Id)).ToListAsync();

            var customDisabilities = form.Disabilities.Where(d => d.IsCustom).Select(d => d.Id).ToList();
            client.CustomDisabilities = await _context.CustomCategories.Where(c => c.Type == CategoryType.Disability && customDisabilities.Contains(c.Id)).ToListAsync();
        }

        public async Task<M.Client?> Find(string userId, long clientId)
        {
            var client = await _context
            .Clients
                .Where(c => c.UserId == userId && c.Id == clientId)
                .Include(c => c.Age)
                .Include(c => c.CustomAge)
                .Include(c => c.Race)
                .Include(c => c.CustomRace)
                .Include(c => c.Gender)
                .Include(c => c.CustomGender)
                .Include(c => c.Setting)
                .Include(c => c.CustomSetting)
                .Include(c => c.SexualOrientation)
                .Include(c => c.CustomSexualOrientation)
                .FirstOrDefaultAsync();
            if (client is null)
            {
                return null;
            }

            await _context.Entry(client).Collection(c => c.Disabilities!).LoadAsync();
            await _context.Entry(client).Collection(c => c.CustomDisabilities!).LoadAsync();
            return client;
        }

        public async Task<List<M.Client>> All(string userId)
        {
            return await _context.Clients.Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task Destroy(string userId, long clientId)
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
