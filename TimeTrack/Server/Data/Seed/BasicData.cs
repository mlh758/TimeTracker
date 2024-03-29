﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.IO;
using TimeTrack.Server.Models;
using TimeTrack.Shared.Enums;

namespace TimeTrack.Server.Data.Seed
{
    public class BasicData
    {
        public static void Seed(string connectionString)
        {
            using var context = new TimeContext(connectionString);
            SeedAssessments(context);
            SeedCategories(context);
            SeedActivityTypes(context);
            context.SaveChanges();
        }

        public static void Seed(string connectionString, string workingDirectory)
        {
            var currentDir = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(workingDirectory);
                Seed(connectionString);
            }
            finally
            {
                Directory.SetCurrentDirectory(currentDir);
            }
        }

        // Assessments pulled from Buros: https://buros.org/tests-reviewed-mental-measurements-yearbook-series
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
            var existingCategories = context.Categories.Where(c => c.UserId == null).ToList();
            using var stream = File.OpenText(@"Data/Seed/categories.txt");

            string? line;
            while ((line = stream.ReadLine()) != null)
            {
                var parts = line.Trim().Split(",").Select(s => s.Trim()).ToList();
                var newCategory = new Category(parts[1], GetCategory(parts[0]));
                if (existingCategories.Any(c => c.Name == newCategory.Name && c.Type == newCategory.Type))
                {
                    continue;
                }
                else
                {
                    context.Add(newCategory);
                }
            }
        }

        private static void SeedActivityTypes(TimeContext context)
        {
            var existingCategories = context.ActivityGrouping.Where(c => c.UserId == null).ToList();
            using var stream = File.OpenText(@"Data/Seed/activities.csv");

            string? line;
            while ((line = stream.ReadLine()) != null)
            {
                var parts = line.Trim().Split(",").Select(s => s.Trim()).ToList();
                var newCategory = new ActivityGrouping(parts[0], GetActivity(parts[1]));
                if (existingCategories.Any(c => c.Name == newCategory.Name && c.GroupingType == newCategory.GroupingType))
                {
                    continue;
                }
                else
                {
                    context.Add(newCategory);
                }
            }
        }

        private static CategoryType GetCategory(string typeName)
        {
            return typeName switch
            {
                "Age" => CategoryType.Age,
                "Race" => CategoryType.Race,
                "SexualOrientation" => CategoryType.SexualOrientation,
                "Disabilities" => CategoryType.Disability,
                "TreatmentSetting" => CategoryType.TreatmentSetting,
                "Gender" => CategoryType.Gender,
                _ => throw new ArgumentException("unrecognized category type in seed file"),
            };
        }

        private static ActivityGroupingType GetActivity(string typeName)
        {
            return typeName switch
            {
                "Intervention" => ActivityGroupingType.Intervention,
                "Assessment" => ActivityGroupingType.Assessment,
                "Support" => ActivityGroupingType.Support,
                "Supervision" => ActivityGroupingType.Supervision,
                _ => throw new ArgumentException($"unrecognized activity type ${typeName} in seed file"),
            };
        }
    }
}
