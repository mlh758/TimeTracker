using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTrack.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack.Shared.Models;

namespace TimeTrack.Server.Services.Tests
{
    [TestClass()]
    public class ScheduleGeneratorTests
    {
        private ScheduleGenerator Setup(DateOnly start, Frequency freq, DateOnly stop, byte weekdays = 0)
        {
            var schedule = new Schedule()
            {
                EndSchedule = stop,
                Frequency = freq,
                DaysOfWeek = weekdays,
            };
            return new ScheduleGenerator(start, schedule);
        }

        private ScheduleGenerator SetupInterval(DateOnly start, Frequency freq, DateOnly stop, ushort interval)
        {
            var schedule = new Schedule()
            {
                EndSchedule = stop,
                Frequency = freq,
                Interval = interval,
            };
            return new ScheduleGenerator(start, schedule);
        }
        [TestMethod()]
        public void Daily()
        {
            var start = new DateOnly(2022, 10, 1);
            var generator = Setup(start, Frequency.Daily, start.AddDays(15));
            var result = generator.Dates().ToList();
            Assert.AreEqual(15, result.Count);
        }

        [TestMethod()]
        public void Daily_EveryOther()
        {
            var start = new DateOnly(2022, 10, 1);
            var generator = SetupInterval(start, Frequency.Daily, start.AddDays(15), 2);
            var result = generator.Dates().ToList();
            Assert.AreEqual(8, result.Count);
        }

        [TestMethod()]
        public void Weekdays()
        {
            var start = new DateOnly(2022, 10, 1);
            var generator = Setup(start, Frequency.Weekdays, start.AddDays(15));
            var result = generator.Dates().ToList();
            Assert.AreEqual(10, result.Count);
            Assert.IsTrue(result.All(r => r.DayOfWeek != DayOfWeek.Saturday && r.DayOfWeek != DayOfWeek.Sunday));
        }

        [TestMethod()]
        public void Monthly_EndOfMonth()
        {
            var start = new DateOnly(2022, 1, 31);
            var generator = Setup(start, Frequency.Monthly, start.AddYears(2));
            var result = generator.Dates().Take(5).ToList();
            var expected = new List<DateOnly>()
            {
                new(2022, 1, 31),
                new(2022, 2, 28),
                new(2022, 3, 31),
                new(2022, 4, 30),
                new(2022, 5, 31)
            };
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void Monthly_EveryOtherMonth()
        {
            var start = new DateOnly(2022, 1, 15);
            var generator = SetupInterval(start, Frequency.Monthly, start.AddYears(2), 2);
            var result = generator.Dates().Take(5).ToList();
            var expected = new List<DateOnly>()
            {
                new(2022, 1, 15),
                new(2022, 3, 15),
                new(2022, 5, 15),
                new(2022, 7, 15),
                new(2022, 9, 15)
            };
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void Weekly()
        {
            // Feb 4, 2022 is a Friday
            var start = new DateOnly(2022, 2, 4);
            // Monday, Wednesday, Friday pattern
            var generator = Setup(start, Frequency.Weekly, start.AddYears(2), 0b_0101_0100);
            var result = generator.Dates().Take(5).ToList();
            var expected = new List<DateOnly>()
            {
                new(2022, 1, 31),
                new(2022, 2, 2),
                new(2022, 2, 4),
                new(2022, 2, 7),
                new(2022, 2, 9)
            };
            CollectionAssert.AreEqual(expected, result);
        }
    }
}