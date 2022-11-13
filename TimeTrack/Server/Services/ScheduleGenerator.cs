using TimeTrack.Shared.Models;
namespace TimeTrack.Server.Services
{
    public class ScheduleGenerator
    {
        private readonly Schedule _schedule;
        private readonly DateOnly _start;
        public ScheduleGenerator(DateOnly startAt, Schedule schedule)
        {
            _schedule = schedule;
            _start = startAt;
        }

        public IEnumerable<DateOnly> Dates()
        {
            switch (_schedule.Frequency)
            {
                case Frequency.Monthly:
                    return Monthly();
                case Frequency.Weekdays:
                    return Weekdays();
                case Frequency.Weekly:
                    return Weekly();
                case Frequency.Daily:
                    return Daily();
                default:
                    throw new ArgumentOutOfRangeException("Unknown schedule frequency");
            }
        }

        private IEnumerable<DateOnly> Daily()
        {
            return Stepper(d => d.AddDays(_schedule.Interval), _start);
        }

        private IEnumerable<DateOnly> Weekdays()
        {
            return Daily().Where(a => a.DayOfWeek != DayOfWeek.Sunday && a.DayOfWeek != DayOfWeek.Saturday);
        }

        private IEnumerable<DateOnly> Monthly()
        {
            return Stepper(d => d.AddMonths(_schedule.Interval), _start).Select(date =>
            {
                if (_start.Day > date.Day)
                {
                    var maxDays = DateTime.DaysInMonth(date.Year, date.Month);
                    return new DateOnly(date.Year, date.Month, Math.Min(maxDays, _start.Day));
                }
                else
                {
                    return date;
                }
            });
        }

        private IEnumerable<DateOnly> Weekly()
        {
            // the weekly pattern starts at the beginning of the week chosen, regardlesss of actual date and relies
            // on the weekdays pattern instead to generate the dates within each week
            var start = _start.AddDays(DayOfWeek.Sunday - _start.DayOfWeek);
            var offsets = Offsets(_schedule.DaysOfWeek);
            foreach (var week in Stepper(d => d.AddDays(7), start))
            {
                foreach (var offset in offsets)
                {
                    yield return week.AddDays(offset);
                }
            }
        }

        private IEnumerable<int> Offsets(byte weekdayPattern)
        {
            var offsets = new List<int>();
            var mask = 0b_1000_0000;
            var offset = 0;
            while (mask > 1)
            {
                if ((weekdayPattern & mask) != 0)
                {
                    offsets.Add(offset);
                }
                offset++;
                mask >>= 1;
            }
            return offsets;
        }

        private IEnumerable<DateOnly> Stepper(Func<DateOnly, DateOnly> step, DateOnly start)
        {
            var current = start;
            while (current < _schedule.EndSchedule)
            {
                yield return current;
                current = step(current);
            }
        }
    }
}
