using System.ComponentModel.DataAnnotations;
using TimeTrack.Shared.ViewModels;


namespace TimeTrack.Shared
{
    public class ActivityForm : IValidatableObject
    {
        [Required]
        public DateTime? Start { get; set; }
        [Required]
        [Range(0, 1440, ErrorMessage = "Duration is limited to the number of minutes in a day (0 - 1440)")] // 1440 is minutes in a 24 hour period
        public int? Duration { get; set; }

        public Schedule? Schedule { get; set; }

        [Required]
        public HashSet<Assessment> Assessments { get; set; }

        public ActivityOwner? Client { get; set; }
        public ActivityOwner? Group { get; set; }

        public ActivityForm()
        {
            Assessments = new HashSet<Assessment>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!(Client is not null ^ Group is not null))
            {
                yield return new ValidationResult(
                    $"You must select either a group or client but not both.",
                    new[] { nameof(Client) });
            }
        }
    }
}
