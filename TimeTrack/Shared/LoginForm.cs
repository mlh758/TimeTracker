using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Shared
{
    public class LoginForm
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
