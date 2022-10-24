using System.ComponentModel.DataAnnotations.Schema;
namespace TimeTrack.Shared.Models
{
    public enum CategoryType
    {
        Age,
        Gender,
        TreatmentSetting,
        Disability,
        Race,
        SexualOrientation
    }
    public class Category
    {
        public int Id { get; set; }
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
    }
}
