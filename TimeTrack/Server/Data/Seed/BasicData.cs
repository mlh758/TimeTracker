using System;
using System.IO;
using TimeTrack.Shared.Models;
using TimeTrack.Server.Repositories;
namespace TimeTrack.Server.Data.Seed
{
    public class BasicData
    {
        public static void Seed(string connectionString)
        {
            using var context = new TimeContext(connectionString);
            if (context.Assessments.Any())
            {
                return;
            }
            using var stream = File.OpenText(@"Data/Seed/assessments.txt");

            string? line;
            while((line = stream.ReadLine()) != null)
            {
                context.Add(new Assessment(line.Trim()));
            }
            var user = new User("Mike Harris", "osiris2918@gmail.com", "password");
            var userRepo = new UserRepository(context);
            var task = userRepo.Create(user);
            task.Wait();
        }
    }
}
