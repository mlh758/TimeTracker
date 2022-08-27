using System;
using System.IO;
using TimeTrack.Shared.Models;
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
            context.Add(new User("Mike Harris", "osiris2918@gmail.com"));
            context.SaveChanges();
        }
    }
}
