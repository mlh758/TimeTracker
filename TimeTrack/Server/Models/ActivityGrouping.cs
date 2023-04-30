using TimeTrack.Shared.Enums;

namespace TimeTrack.Server.Models
{
    public class ActivityGrouping
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ActivityGroupingType GroupingType { get; set; }
        public User? User { get; set; }
        public string? UserId { get; set; }

        public ICollection<Activity>? Activities { get; set; }

        public ActivityGrouping(string name, ActivityGroupingType groupingType)
        {
            Name = name;
            GroupingType = groupingType;
        }
    }
}
