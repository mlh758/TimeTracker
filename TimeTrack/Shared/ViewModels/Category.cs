using TimeTrack.Shared.Enums;

namespace TimeTrack.Shared.ViewModels
{
    public class Category
    {
        public long Id { get; init; }
        public string Name { get; init; }
        public CategoryType Type { get; init; }
        public bool IsCustom { get; init; } = false;
        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Category c = (Category)obj;
                return (Id == c.Id);
            }
        }
        public Category(string name)
        {
            Name = name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
