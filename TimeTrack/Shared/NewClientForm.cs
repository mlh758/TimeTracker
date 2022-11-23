using System.ComponentModel.DataAnnotations;
using TimeTrack.Shared.ViewModels;

namespace TimeTrack.Shared
{
    public class NewClientForm
    {
        [Required]
        [StringLength(16, ErrorMessage = "Abbreviations must be at most 16 characters")]
        public string? Abbreviation { get; set; }
        [Required]
        public Category? Age { get; set; }
        [Required]
        public Category? Setting { get; set; }
        [Required]
        public Category? SexualOrientation { get; set; }
        [Required]
        public Category? Gender { get; set; }
        public IEnumerable<Category> Disabilities { get; set; } = Enumerable.Empty<Category>();
        [Required]
        public Category? Race { get; set; }
    }
}
