﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int Id { get; set; }
        [Comment("Date schedule should terminte on, inclusive")]
        [Column(TypeName = "date")]
        public DateTime EndSchedule { get; set; }
        [Comment("Bits for day of week Sunday to Saturday starting at the most significant bit.")]
        public byte DaysOfWeek { get; set; }
        [Comment("Gap between events. e.g 2 could be for every othe week")]
        [Range(1, 16, ErrorMessage = "Gap between events must be between 1 and 16")]
        public ushort Interval { get; set; } = 1;
        public Frequency Frequency { get; set; }
    }
}
