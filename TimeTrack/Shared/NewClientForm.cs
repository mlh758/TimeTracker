using System.ComponentModel.DataAnnotations;

namespace TimeTrack.Shared
{
    public class NewClientForm
    {
        [Required]
        [StringLength(16, ErrorMessage = "Abbreviations must be at most 16 characters")]
        public string? Abbreviation { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }
}
