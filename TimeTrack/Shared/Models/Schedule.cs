using Microsoft.EntityFrameworkCore;

namespace TimeTrack.Shared.Models
{
    public enum Frequency
    {
        Daily,
        Weekdays,
        Weekly,
        Monthly,
    }
    /*
     * Keeps track of the scheduling rule that activities are generated for. Useful for generating
     * activities and to know which ones to delete if the rules are modified.
     */
    public class Schedule
    {
        
        [Comment("Date schedule should terminte on, inclusive")]
        public DateOnly EndSchedule { get; set; }
        [Comment("Bits for day of week Sunday to Saturday starting at the most significant bit.")]
        public byte DaysOfWeek { get; set; }
        [Comment("Gap between events. e.g 2 could be for every othe week")]
        public ushort Interval { get; set; } = 1;
        public Frequency Frequency { get; set; }
    }
}
