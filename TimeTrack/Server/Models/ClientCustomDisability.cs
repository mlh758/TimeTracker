namespace TimeTrack.Server.Models
{
    public class ClientCustomDisability
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public long DisabilityId {  get; set; }
        public CustomCategory Disability { get; set; }
    }
}
