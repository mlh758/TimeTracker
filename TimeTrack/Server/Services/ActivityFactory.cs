using TimeTrack.Client.Pages;
using TimeTrack.Server.Models;
using TimeTrack.Shared;

namespace TimeTrack.Server.Services
{
    public class ActivityFactory
    {
        public static Activity UpdateFromForm(Activity currentActivity, ActivityForm form, IEnumerable<Assessment> selectedAssessments)
        {
            if (form.Group is null && form.Client is null)
            {
                throw new ArgumentException("client or group is required for activity");
            }
            if (form.Group is not null)
            {
                currentActivity.GroupId = form.Group.Id;
            }
            if (form.Client is not null)
            {
                currentActivity.ClientId = form.Client.Id;
            }
            if (form.Schedule is not null)
            {
                currentActivity.Schedule = form.Schedule;
            }
            currentActivity.Start = form.Start!.Value;
            currentActivity.ClinicalHours = form.ClinicalHours!.Value;
            currentActivity.Assessments = selectedAssessments.ToList();
            currentActivity.UsedInIntegratedReport = form.UsedInIntegratedReport;
            currentActivity.UsedInResearch = form.UsedInResearch;
            currentActivity.ClinicallyScored = form.ClinicallyScored;
            return currentActivity;
        }
        public static Activity FromForm(ActivityForm form, IEnumerable<Assessment> selectedAssessments)
        {
            var newActivity = new Activity();
            return UpdateFromForm(newActivity, form, selectedAssessments);
        }
    }
}
