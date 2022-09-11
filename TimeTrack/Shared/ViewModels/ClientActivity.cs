using TimeTrack.Shared.Models;

namespace TimeTrack.Shared.ViewModels
{
    public readonly struct ActivityClient
    {
        public string Abbreviation { get; init; }
    }
    public readonly struct ClientActivity
    {
        public DateTime Start { get; init; }
        public DateTime End { get; init; }
        public ActivityClient Client { get; init; }
        public List<Assessment> Assessments { get; init; }
    }
}
