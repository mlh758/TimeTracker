namespace TimeTrack.Shared.ViewModels
{
    public readonly struct Client
    {
        public string Abbreviation { get; init; }
        public int Id { get; init; }

        public Category Age { get; init; }
        public Category Setting { get; init; }
        public Category SexualOrientation { get; init; }
        public Category Race { get; init; }
        public Category Gender { get; init; }
        public List<Category> Disabilities { get; init; }
    }

    public readonly struct SummaryClient
    {
        public string Abbreviation { get; init; }
        public int Id { get; init; }
    }
}
