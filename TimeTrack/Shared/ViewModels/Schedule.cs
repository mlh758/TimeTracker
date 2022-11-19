using System.ComponentModel.DataAnnotations;
using TimeTrack.Shared.Enums;

namespace TimeTrack.Shared.ViewModels
{
    public class Schedule
    {
        public int Id { get; set; }
        public DateTime EndSchedule { get; set; }
        public byte DaysOfWeek { get; set; }
        [Range(1, 16, ErrorMessage = "Gap between events must be between 1 and 16")]
        public ushort Interval { get; set; } = 1;
        public Frequency Frequency { get; set; }
    }
}
