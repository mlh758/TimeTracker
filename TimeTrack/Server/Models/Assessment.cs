using Microsoft.EntityFrameworkCore;

namespace TimeTrack.Server.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        
        [Comment("Assessment names from buros.org mental measurements yearbook series")]
        public string Name {  get; set; }
        public List<Activity>? ClientActivities { get; set; }

        public Assessment(string name)
        {
            Name = name;
        }
    }
}
