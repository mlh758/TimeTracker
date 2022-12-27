using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VM = TimeTrack.Shared.ViewModels;

namespace TimeTrack.Server.Models
{
    [Index(nameof(UserId), nameof(Abbreviation), IsUnique = true)]
    public class Client
    {
        public long Id { get; set; }
        [MaxLength(16)]
        public string Abbreviation { get; set; }

        public long AgeId { get; set; }
        public Category? Age { get; set; }
        public long SettingId { get; set; }
        public Category? Setting { get; set; }
        public long SexualOrientationId { get; set; }
        public Category? SexualOrientation { get; set; }
        public long RaceId { get; set; }
        public Category? Race { get; set; }
        public long GenderId { get; set; }
        public Category? Gender { get; set; }
        public List<Category>? Disabilities { get; set; }

        public string UserId { get; set; }
        public User? User { get; set; }

        public IEnumerable<Group> Groups { get; set; }

        public List<Activity>? Activities { get; set; }

        public Client(string abbreviation)
        {
            Abbreviation = abbreviation;
        }

        public static implicit operator VM.ActivityOwner(Client c) => new() { Id = c.Id, Name = c.Abbreviation };
    }
}
