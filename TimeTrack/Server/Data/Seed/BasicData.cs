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
            SeedAssessments(context);
            SeedCategories(context);
            context.SaveChanges();
        }

        private static void SeedAssessments(TimeContext context)
        {
            if (context.Assessments.Any())
            {
                return;
            }
            using var stream = File.OpenText(@"Data/Seed/assessments.txt");

            string? line;
            while ((line = stream.ReadLine()) != null)
            {
                context.Add(new Assessment(line.Trim()));
            }
        }

        // Categories pulled from https://www.appic.org/Portals/0/downloads/AAPI/T2T%20Sample%20PDF.pdf
        // AAPI 2020-2021 Application
        private static void SeedCategories(TimeContext context)
        {
            if(context.Categories.Any())
            {
                return;
            }
            using var stream = File.OpenText(@"Data/Seed/categories.txt");

            string? line;
            while ((line = stream.ReadLine()) != null)
            {
                var parts = line.Trim().Split(",").Select(s => s.Trim()).ToList();
                context.Add(new Category(parts[1], GetCategory(parts[0])));
            }
        }

        private static CategoryType GetCategory(string typeName)
        {
            switch(typeName)
            {
                case "Age":
                    return CategoryType.Age;
                case "Race":
                    return CategoryType.Race;
                case "SexualOrientation":
                    return CategoryType.SexualOrientation;
                case "Disabilities":
                    return CategoryType.Disability;
                case "TreatmentSetting":
                    return CategoryType.TreatmentSetting;
                case "Gender":
                    return CategoryType.Gender;
                default:
                    throw new ArgumentException("unrecognized category type in seed file");
            }
        }
    }
}
