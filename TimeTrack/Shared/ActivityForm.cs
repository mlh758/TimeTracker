using System.ComponentModel.DataAnnotations;
using TimeTrack.Shared.Models;


namespace TimeTrack.Shared
{
    public class ActivityForm
    {
        [Required]
        public DateTime? Start { get; set; }
        [Required]
        public int? Duration { get; set; }

        public int? ClientId;

        private Client? _client;

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
    }
}
