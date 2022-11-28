namespace TimeTrack.Server.Models
{
    public class Group
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public ICollection<Client>? Clients { get; set; }
        public ICollection<Activity>? Activities { get; set; }
    }
}
