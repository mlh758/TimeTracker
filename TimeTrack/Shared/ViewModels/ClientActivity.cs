namespace TimeTrack.Shared.ViewModels
{
    public readonly struct Client
    {
        public string Abbreviation { get; init; }
    }

    public readonly struct Assessment
    {
        public string Name { get; init; }
    }

    public readonly struct ClientActivity
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public Client Client { get; init; }
        public List<Assessment> Assessments { get; init; }
    }
}
