using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTrack.Shared.Models
{
    /*
     * I considered implementing this based on the iCal spec:
     * https://icalendar.org/iCalendar-RFC-5545/3-8-5-3-recurrence-rule.html
     * But that is more optimal for efficiently getting events onto a calendar over some limited span.
     * Set up your rules, pick a span, and fill in the events based on the rules. A daily activity for a year would only
     * need 1 row.
     * 
     * The main use for this data though is for reporting on several years of it to fill out paperwork. Most activities
     * are only a couple days a week, and it's much easier to just ask for and aggregate records from the DB than pull rules
     * and then generate that data in memory, and then aggregate it.
     */
    public class Activity
    {
        public int Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime Start { get; set; }
        [Comment("Duration in minutes")]
        public int Duration { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public Schedule? Schedule { get; set; }

        public List<Assessment>? Assessments { get; set; }
        public Activity() { }
        public Activity(Activity previous)
        {
            Start = previous.Start;
            Duration = previous.Duration;
            ClientId = previous.ClientId;
            Schedule = previous.Schedule;
            Assessments = previous.Assessments;
        }
    }
}
