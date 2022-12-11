using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TimeTrack.Server.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<CustomCategory>? CustomCategories { get; set; }
        public IEnumerable<UserCredential>? Credentials { get; set; }

    }
}
