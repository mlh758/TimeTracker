using TimeTrack.Shared.Enums;
using VM = TimeTrack.Shared.ViewModels;
namespace TimeTrack.Server.Models
{
    
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }

        
        public List<Client>? DisabledClients { get; set; }

        public List<Client>? AgedClients { get; set; }

        public List<Client>? SettingClients { get; set; }

        public List<Client>? GenderedClients { get; set; }

        public List<Client>? SexualOrientationClients { get; set; }

        public List<Client>? RaceClients { get; set; }

        public Category(string name, CategoryType type)
        {
            Name = name;
            Type = type;
        }

        public override string ToString()
        {
            return Name;
        }
        public static implicit operator VM.Category(Category c) => new(c.Name) { Id = c.Id, Type = c.Type };
    }
}
