using TimeTrack.Shared.Enums;

namespace TimeTrack.Shared.ViewModels
{
    public readonly struct CategoryCount
    {
        public string Name { get; init; }
        public int Count { get; init; }
    }
    public readonly struct Group
    {
        public string Name { get; init; }
        public ActivityOwner[] Clients { get; init; }
        public List<Dictionary<CategoryType, CategoryCount>> Demographics { get; init; }
    }
}
