namespace TimeTrack.Shared.ViewModels
{
    public readonly struct Assessment
    {
        public string Name { get; init; }
    }

    public readonly struct ClientActivity
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public SummaryClient Client { get; init; }
        public List<Assessment> Assessments { get; init; }
    }
}
