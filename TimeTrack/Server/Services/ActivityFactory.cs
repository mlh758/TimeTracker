using TimeTrack.Client.Pages;
using TimeTrack.Server.Models;
using TimeTrack.Shared;

namespace TimeTrack.Server.Services
{
    public class ActivityFactory
    {
        public static Activity UpdateFromForm(Activity currentActivity, ActivityForm form, IEnumerable<Assessment> selectedAssessments)
        {
            if (form.Group is not null)
            {
                currentActivity.GroupId = form.Group.Id;
            }
            if (form.Client is not null)
            {
                currentActivity.ClientId = form.Client.Id;
            }
            // We do not currently support changing a schedule
            if (form.Schedule is not null && currentActivity.Schedule is null)
            {
                currentActivity.Schedule = form.Schedule;
            }
            currentActivity.ActivityGroupingId = form.Grouping!.Id;
            currentActivity.Start = form.Start!.Value;
            currentActivity.ClinicalHours = form.ClinicalHours!.Value;
            currentActivity.Assessments = selectedAssessments.ToList();
            currentActivity.UsedInIntegratedReport = form.UsedInIntegratedReport;
            currentActivity.UsedInResearch = form.UsedInResearch;
            currentActivity.ClinicallyScored = form.ClinicallyScored;
            return currentActivity;
        }
        public static Activity FromForm(string userId, ActivityForm form, IEnumerable<Assessment> selectedAssessments)
        {
            var newActivity = new Activity(userId);
            return UpdateFromForm(newActivity, form, selectedAssessments);
        }
    }
}
