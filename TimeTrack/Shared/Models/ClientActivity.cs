namespace TimeTrack.Shared.Models
{
    public class ClientActivity
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int UserId {  get; set; }
        public User? User { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public List<Assessment>? Assessments { get; set; }
    }
}
