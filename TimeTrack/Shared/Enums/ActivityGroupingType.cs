namespace TimeTrack.Shared.Enums
{
    public enum ActivityGroupingType
    {
        Intervention,
        Assessment,
        Support,
        Supervision
    }

    public static class ActivityGroupingTypeDisplay
    {
        public static string Show(ActivityGroupingType type)
        {
            return type switch
            {
                ActivityGroupingType.Intervention => "Intervention",
                ActivityGroupingType.Assessment => "Assessment",
                ActivityGroupingType.Support => "Support",
                ActivityGroupingType.Supervision=> "Supervision",
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }
    }
}
