namespace TimeTrack.Server.Models
{
    public class Group
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Client>? Clients { get; set; }
        public IEnumerable<Activity>? Activities { get; set; }
    }
}
