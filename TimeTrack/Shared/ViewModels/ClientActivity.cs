namespace TimeTrack.Shared.ViewModels
{
    public readonly struct Assessment
    {
        public string Name { get; init; }
    }

    public readonly struct ClientActivity
    {
        public DateTime Start { get; init; }
        public int Duration { get; init; }
        public SummaryClient Client { get; init; }
        public List<Assessment> Assessments { get; init; }
    }
}
