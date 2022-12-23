using TimeTrack.Server.Models;
using TimeTrack.Shared;

namespace TimeTrack.Server.Services
{
    public class ActivityFactory
    {
        public static Activity FromForm(ActivityForm form, IEnumerable<Assessment> selectedAssessments)
        {
            Activity newActivity;
            if (form.Group is null && form.Client is null)
            {
                throw new ArgumentException("client or group is required for activity");
            }
            if (form.Group is not null)
            {
                newActivity = new Activity()
                {
                    GroupId = form.Group.Id,
                };
            }
            else
            {
                newActivity = new Activity()
                {
                    ClientId = form.Client!.Id,
                };
            }
            if (form.Schedule is not null)
            {
                newActivity.Schedule = form.Schedule;
            }
            newActivity.Start = form.Start!.Value;
            newActivity.ClinicalHours = form.ClinicalHours!.Value;
            newActivity.Assessments = selectedAssessments.ToList();
            return newActivity;
        }
    }
}
