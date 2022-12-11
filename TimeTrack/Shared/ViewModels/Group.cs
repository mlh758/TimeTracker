namespace TimeTrack.Shared.ViewModels
{
    public readonly struct Group
    {
        public string Name { get; init; }
        public ActivityOwner[] Clients { get; init; }
    }
}
