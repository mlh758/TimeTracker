using System.ComponentModel.DataAnnotations;
using TimeTrack.Shared.ViewModels;


namespace TimeTrack.Shared
{
    public class ActivityForm
    {
        [Required]
        public DateTime? Start { get; set; }
        [Required]
        [Range(0, 1440, ErrorMessage = "Duration is limited to the number of minutes in a day (0 - 1440)")] // 1440 is minutes in a 24 hour period
        public int? Duration { get; set; }

        public Schedule? Schedule { get; set; }

        public int? ClientId;

        private Client? _client;

        [Required]
        public HashSet<Assessment> Assessments { get; set; }


        [Required]
        public Client? Client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
                ClientId = value?.Id;
            }
        }

        public ActivityForm()
        {
            Assessments = new HashSet<Assessment>();
        }
    }
}
