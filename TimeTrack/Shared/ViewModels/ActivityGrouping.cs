using TimeTrack.Shared.Enums;

namespace TimeTrack.Shared.ViewModels
{
    public class ActivityGrouping
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public ActivityGroupingType Type { get; init; }
        public override string ToString()
        {
            return Name;
        }

        public ActivityGrouping(string name)
        {
            Name = name;
        }
    }
}
