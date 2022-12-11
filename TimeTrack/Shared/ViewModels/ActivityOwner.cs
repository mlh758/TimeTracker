namespace TimeTrack.Shared.ViewModels
{
    public class ActivityOwner : IEquatable<ActivityOwner?>
    {
        public string Name { get; init; }
        public long Id { get; init; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ActivityOwner);
        }

        public bool Equals(ActivityOwner? other)
        {
            return other is not null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public static bool operator ==(ActivityOwner? left, ActivityOwner? right)
        {
            return EqualityComparer<ActivityOwner>.Default.Equals(left, right);
        }

        public static bool operator !=(ActivityOwner? left, ActivityOwner? right)
        {
            return !(left == right);
        }
    }
}
