﻿using TimeTrack.Server.Data;
using M = TimeTrack.Shared.Models;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Shared;


namespace TimeTrack.Server.Repositories
{
    public interface IClientRepository
    {
        public Task<M.Client?> Find(int userId, int clientId);
        public Task<List<M.Client>> All(int userId);
        public Task<M.Client> Create(int userId, NewClientForm form);
        public Task Destroy(int userId, int clientId);
    }
    public class ClientRepository : IClientRepository
    {

        private readonly TimeContext _context;
        public ClientRepository(TimeContext context)
        {
            _context = context;
        }

        public async Task<M.Client> Create(int userId, NewClientForm clientData)
        {
            var newClient = new M.Client(clientData.Abbreviation!)
            {
                UserId = userId,
                AgeId = clientData.Age!.Id,
                SettingId = clientData.Setting!.Id,
                SexualOrientationId = clientData.SexualOrientation!.Id,
                GenderId = clientData.Gender!.Id,
                RaceId = clientData.Race!.Id
            };
            if (clientData.Disabilities is not null)
            {
                var disabilityIds = clientData.Disabilities.Select(d => d.Id).ToList();
                newClient.Disabilities = await _context.Categories.Where(c => c.Type == Shared.Models.CategoryType.Disability && disabilityIds.Contains(c.Id)).ToListAsync();
            }
            _context.Add(newClient);
            await _context.SaveChangesAsync();
            return newClient;
        }

        public async Task<M.Client?> Find(int userId, int clientId)
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

        public async Task<List<M.Client>> All(int userId)
        {
            return await _context.Clients.Where(c => c.UserId == userId).ToListAsync();
        }

        async Task IClientRepository.Destroy(int userId, int clientId)
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