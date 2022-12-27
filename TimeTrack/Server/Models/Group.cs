using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VM = TimeTrack.Shared.ViewModels;

namespace TimeTrack.Server.Models
{
    [Index(nameof(UserId), nameof(Name), IsUnique = true)]
    public class Group
    {
        public long Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }

        public string UserId { get; set; }
        public User? User { get; set; }

        public Group(string userId)
        {
            UserId = userId;
        }

        public IEnumerable<Client>? Clients { get; set; }
        public IEnumerable<Activity>? Activities { get; set; }

        public static implicit operator VM.ActivityOwner(Group? g) => new() { Id = g.Id, Name = g.Name };
    }
}
