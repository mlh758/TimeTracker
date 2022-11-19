namespace TimeTrack.Shared.ViewModels
{
    public class Client
    {
        public string Abbreviation { get; set; } = "";
        public int Id { get; set; }

        public Category? Age { get; set; }
        public Category? Setting { get; set; }
        public Category? SexualOrientation { get; set; }
        public Category? Race { get; set; }
        public Category? Gender { get; set; }
        public List<Category> Disabilities { get; set; } = new List<Category>();
    }

    public readonly struct SummaryClient
    {
        public string Abbreviation { get; init; }
        public int Id { get; init; }
    }
}
