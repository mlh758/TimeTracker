namespace TimeTrack.Shared.ViewModels
{ 
    public readonly struct SummaryActivity
    {
        public DateTime Start { get; init; }
        public int Duration { get; init; }
        public ActivityOwner Owner { get; init; }
        public List<Assessment> Assessments { get; init; }
    }
}
