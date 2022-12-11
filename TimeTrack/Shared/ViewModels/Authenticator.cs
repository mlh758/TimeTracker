namespace TimeTrack.Shared.ViewModels
{
    public readonly struct Authenticator
    {
        public string Name { get; init; }
        public DateTime Registered { get; init; }
        public int Id { get; init; }
    }
}
