using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Shared.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Abbreviation { get; set; }

        public int AgeId { get; set; }
        public Category? Age { get; set; }
        public int SettingId { get; set; }
        public Category? Setting { get; set; }
        public int SexualOrientationId { get; set; }
        public Category? SexualOrientation { get; set; }
        public int RaceId { get; set; }
        public Category? Race { get; set; }
        public int GenderId { get; set; }
        public Category? Gender { get; set; }
        public List<Category>? Disabilities { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public List<Activity>? Activities { get; set; }

        public Client(string abbreviation)
        {
            Abbreviation = abbreviation;
        }
    }
}
