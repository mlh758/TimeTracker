using TimeTrack.Shared.Enums;
using VM = TimeTrack.Shared.ViewModels;

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

        public List<Client>? DisabledClients { get; set; }
        public List<ClientCustomDisability>? ClientCustomDisabilities { get; set; }

        public List<Client>? AgedClients { get; set; }

        public List<Client>? SettingClients { get; set; }

        public List<Client>? GenderedClients { get; set; }

        public List<Client>? SexualOrientationClients { get; set; }

        public List<Client>? RaceClients { get; set; }
        public CustomCategory(string name, CategoryType type, string userId)
        {
            Name = name;
            Type = type;
            UserId = userId;
        }
        public static implicit operator VM.Category(CustomCategory c) => new(c.Name) { Id = c.Id, Type = c.Type, IsCustom = true };
    }
}
