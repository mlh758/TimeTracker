using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TimeTrack.Shared.ViewModels;

namespace TimeTrack.Server.Models
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
        public long Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime Start { get; set; }
        [Precision(5,2)]
        public decimal ClinicalHours { get; set; }
        public long? ClientId { get; set; }
        public Client? Client { get; set; }

        public long? GroupId { get; set; }
        public Group? Group { get; set; }

        public Schedule? Schedule { get; set; }
        public int? ScheduleId { get; set; }

        public List<Assessment>? Assessments { get; set; }
        public bool UsedInResearch { get; set; }
        public bool ClinicallyScored { get; set; }
        public bool UsedInIntegratedReport { get; set; }
        public Activity() { }
        public Activity(Activity previous)
        {
            Start = previous.Start;
            ClinicalHours = previous.ClinicalHours;
            ClientId = previous.ClientId;
            GroupId = previous.GroupId;
            Schedule = previous.Schedule;
            Assessments = previous.Assessments;
        }

        public ActivityOwner GetOwner()
        {
            if (Group is not null)
            {
                return new ActivityOwner() {  Id = Group.Id, Name = Group.Name };
            }
            if (Client is not null)
            {
                return new ActivityOwner() { Id = Client.Id, Name = Client.Abbreviation };
            }
            throw new Exception("activity or client must be set");
        }
    }
}
