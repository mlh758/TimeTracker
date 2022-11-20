using TimeTrack.Shared.Enums;

namespace TimeTrack.Server.Models
{
    /*
     * This is the same as Category except users can define their own custom values
     * for these and select them from the category dropdowns.
     */
    public class CustomCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        public User? User { get; set; }
        public string UserId { get; set; }
        public CustomCategory(string name, CategoryType type, string userId)
        {
            Name = name;
            Type = type;
            UserId = userId;
        }
    }
}
