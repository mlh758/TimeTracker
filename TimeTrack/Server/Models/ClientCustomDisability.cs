namespace TimeTrack.Server.Models
{
    public class ClientCustomDisability
    {
        public long ClientId { get; set; }
        public Client Client { get; set; }

        public long DisabilityId {  get; set; }
        public CustomCategory Disability { get; set; }
    }
}
