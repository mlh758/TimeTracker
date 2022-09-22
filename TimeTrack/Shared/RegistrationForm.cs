using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Shared
{
    public class RegistrationForm
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password and confirmation must match")]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
