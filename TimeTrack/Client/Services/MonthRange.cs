using System.Collections;

namespace TimeTrack.Client.Services
{
    public class MonthRange : IEnumerator<DateTime>, IEnumerable<DateTime>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private DateTime _current;

        public MonthRange(DateTime startDate)
        {
            var startAt = new DateTime(startDate.Year, startDate.Month, 1);
            // offset from Sunday, minus one more because foreach starts by calling MoveNext
            var offset = DayOfWeek.Sunday - startAt.DayOfWeek - 1;
            _startDate = startAt.AddDays(offset);
            _current = _startDate;
            var endAt = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
            offset = DayOfWeek.Saturday - endAt.DayOfWeek;
            _endDate = endAt.AddDays(offset);
        }

        public DateTime Current => _current;

        object IEnumerator.Current => _current;

        public bool MoveNext()
        {
            var next = _current.AddDays(1);
            if (next <= _endDate)
            {
                _current = next;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _current = _startDate;
        }


        public void Dispose()
        {
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
