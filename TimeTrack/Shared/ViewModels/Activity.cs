namespace TimeTrack.Shared.ViewModels
{
    public class Activity
    {
        public int Id { get; set; }

        public DateTime Start { get; set; }

        public int Duration { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public Schedule? Schedule { get; set; }

        public List<Assessment>? Assessments { get; set; }
    }
}
