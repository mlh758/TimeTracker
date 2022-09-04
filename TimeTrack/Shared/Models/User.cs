using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [MaxLength(8), Column(TypeName = "Binary")]
        public byte[]? Salt { get; set; }

        public string? PasswordSalt {  get => Salt is null ? null : Convert.ToBase64String(Salt); }
        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
